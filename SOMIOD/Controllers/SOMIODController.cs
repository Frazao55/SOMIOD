using Middleware.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Xml;
using uPLibrary.Networking.M2Mqtt;
using System.Text;

namespace SOMIOD.Controllers
{
    [RoutePrefix("api/somiod")]
    public class SOMIODController : ApiController
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SOMIOD.Properties.Settings.ConnStr"].ConnectionString;
        MqttClient mcClient = null;

        /*
        POST http://<domain:port>/api/somiod

        <resource res_type="application">
            <name>Ventilation</name>
        </resource>
         */
        [HttpPost]
        [Route("")]
        public IHttpActionResult PostApplication()
        {
            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            string xml = bodyStream.ReadToEnd();

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
                XmlNode nameNode = doc.SelectSingleNode("/resource/name");

                SqlConnection conn = null;
                SqlDataReader reader;
                string name = nameNode?.InnerText;
                try
                {
                    conn = new SqlConnection(connectionString);
                    conn.Open();

                    SqlCommand cmd;
                    if (name != null)
                    {
                        cmd = new SqlCommand("INSERT INTO Applications (Name, CreationDT) OUTPUT INSERTED.ID, INSERTED.NAME VALUES (@Name, @CreationDT)", conn);
                        cmd.Parameters.AddWithValue("@Name", name);
                    }
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO Applications (CreationDT) OUTPUT INSERTED.ID, INSERTED.NAME VALUES (@CreationDT)", conn);
                    }

                    DateTime creationDT = DateTime.Now;
                    cmd.Parameters.AddWithValue("@CreationDT", creationDT);

                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int insertedId = (int)reader["Id"];
                        string insertedName = (string)reader["Name"];
                        return Ok(new Application() { Id = insertedId, Name = insertedName, CreationDT = creationDT });
                    }
                    else
                    {
                        return InternalServerError(new Exception("Error inserting application"));
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
                finally
                {
                    conn?.Close();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error processing XML: " + ex.Message);
            }
        }

        /*
        GET http://<domain:port>/api/somiod
        somiod-discover: application

        The discover operation is represented by the GET HTTP verb and by the presence of an HTTP header called "somiod-discover: <res_type>" 
        Where <res_type> could be application, container, data, and subscription.
        */
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllApplications()
        {
            string resType = HttpContext.Current.Request.Headers["somiod-discover"];
            if (resType is null)
            {
                return BadRequest("Missing somiod-discover header");
            }

            if (resType == "application")
            {
                SqlConnection conn = null;
                SqlDataReader reader = null;
                try
                {
                    conn = new SqlConnection(connectionString);
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Applications", conn);

                    reader = cmd.ExecuteReader();
                    List<string> names = new List<string>();
                    while (reader.Read())
                    {
                        names.Add((string)reader["Name"]);
                    }
                    return Ok(names);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
                finally
                {
                    reader?.Close();
                    conn?.Close();
                }
            }
            else
            {
                return BadRequest("Invalid somiod-discover header");
            }
        }

        /*
        DELETE http://<domain:port>/api/somiod/applicationX
        */
        [HttpDelete]
        [Route("{application}")]
        public IHttpActionResult DeleteApplication(string application)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM Applications WHERE Name = @Name", conn);
                cmd.Parameters.AddWithValue("@Name", application);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            finally
            {
                conn?.Close();
            }
        }

        /*
        PUT http://<domain:port>/api/somiod/applicationX

        <resource res_type="application">
            <name>Lighting</name>
        </resource>
        */
        [HttpPut]
        [Route("{application}")]
        public IHttpActionResult PutApplication(string application)
        {
            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            string xml = bodyStream.ReadToEnd();

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
                XmlNode nameNode = doc.SelectSingleNode("/resource/name");

                if (nameNode is null)
                {
                    return BadRequest("Missing name element");
                }

                SqlConnection conn = null;
                string newName = nameNode.InnerText;
                try
                {
                    conn = new SqlConnection(connectionString);
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE Applications SET Name = @NewName WHERE Name = @Name", conn);
                    cmd.Parameters.AddWithValue("@Name", application);
                    cmd.Parameters.AddWithValue("@NewName", newName);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
                finally
                {
                    conn?.Close();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error processing XML: " + ex.Message);
            }
        }


        /*
        POST http://<domain:port>/api/somiod/applicationX

        <resource res_type="container">
            <name>fan</name>
        </resource>
        */
        [HttpPost]
        [Route("{application}")]
        public IHttpActionResult PostContainer(string application)
        {
            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            string xml = bodyStream.ReadToEnd();

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
                XmlNode nameNode = doc.SelectSingleNode("/resource/name");

                SqlConnection conn = null;
                SqlDataReader reader;
                string name = nameNode?.InnerText;
                try
                {
                    conn = new SqlConnection(connectionString);
                    conn.Open();

                    SqlCommand insertCmd;
                    if (name != null)
                    {
                        insertCmd = new SqlCommand("INSERT INTO Containers (Name, Parent, CreationDT) OUTPUT INSERTED.ID, INSERTED.NAME, INSERTED.PARENT VALUES (@Name, (SELECT Id FROM Applications WHERE Name = @Application), @CreationDT)", conn);
                        insertCmd.Parameters.AddWithValue("@Name", name);
                    }
                    else
                    {
                        // If the name is not specified, generate a random name
                        insertCmd = new SqlCommand("INSERT INTO Containers (Parent, CreationDT) OUTPUT INSERTED.ID, INSERTED.NAME, INSERTED.PARENT VALUES ((SELECT Id FROM Applications WHERE Name = @Application), @CreationDT)", conn);
                    }
                    insertCmd.Parameters.AddWithValue("@Application", application);

                    DateTime creationDT = DateTime.Now;
                    insertCmd.Parameters.AddWithValue("@CreationDT", creationDT);

                    reader = insertCmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int insertedId = (int)reader["Id"];
                        string insertedName = (string)reader["Name"];
                        int insertedParent = (int)reader["Parent"];
                        return Ok(new Container() { Id = insertedId, Name = insertedName, Parent = insertedParent, CreationDT = creationDT });
                    }
                    else
                    {
                        return InternalServerError(new Exception("Error inserting container"));
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
                finally
                {
                    conn?.Close();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error processing XML: " + ex.Message);
            }
        }

        /*
        DELETE http://<domain:port>/api/somiod/applicationX/containerY
        */
        [HttpDelete]
        [Route("{application}/{container}")]
        public IHttpActionResult DeleteContainer(string application, string container)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                // Get the container id
                SqlCommand selectCmd = new SqlCommand("SELECT Id FROM Containers WHERE Name = @Name AND Parent = (SELECT Id FROM Applications WHERE Name = @Application)", conn);
                selectCmd.Parameters.AddWithValue("@Name", container);
                selectCmd.Parameters.AddWithValue("@Application", application);
                int containerId = (int)selectCmd.ExecuteScalar();

                // Delete the subscriptions which are children of the container
                SqlCommand deleteSubscriptionsCmd = new SqlCommand("DELETE FROM Subscriptions WHERE Parent = @Parent", conn);
                deleteSubscriptionsCmd.Parameters.AddWithValue("@Parent", containerId);
                deleteSubscriptionsCmd.ExecuteNonQuery();

                // Delete the data which are children of the container
                SqlCommand deleteDataCmd = new SqlCommand("DELETE FROM Data WHERE Parent = @Parent", conn);
                deleteDataCmd.Parameters.AddWithValue("@Parent", containerId);
                deleteDataCmd.ExecuteNonQuery();

                // Delete the container
                SqlCommand deleteContainerCmd = new SqlCommand("DELETE FROM Containers WHERE Id = @Id", conn);
                deleteContainerCmd.Parameters.AddWithValue("@Id", containerId);
                int rowsAffected = deleteContainerCmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            finally
            {
                conn?.Close();
            }
        }

        /*
        PUT http://<domain:port>/api/somiod/applicationX/containerY

        <resource res_type="container">
            <name>light_bulb</name>
        </resource>
        */
        [HttpPut]
        [Route("{application}/{container}")]
        public IHttpActionResult PutContainer(string application, string container)
        {
            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            string xml = bodyStream.ReadToEnd();

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
                XmlNode nameNode = doc.SelectSingleNode("/resource/name");

                if (nameNode is null)
                {
                    return BadRequest("Missing name element");
                }

                SqlConnection conn = null;
                string newName = nameNode.InnerText;
                try
                {
                    conn = new SqlConnection(connectionString);
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE Containers SET Name = @NewName WHERE Name = @Name AND Parent = (SELECT Id FROM Applications WHERE Name = @Application)", conn);
                    cmd.Parameters.AddWithValue("@Name", container);
                    cmd.Parameters.AddWithValue("@NewName", newName);
                    cmd.Parameters.AddWithValue("@Application", application);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
                finally
                {
                    conn?.Close();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error processing XML: " + ex.Message);
            }
        }

        /*
        GET http://<domain:port>/api/somiod/applicationX/containerY

        Return the container information or: 
            - if the header somiod-discover: subscription is present, return the subscriptions of the container
            - if the header somiod-discover: data is present, return the data of the container.
        */
        [HttpGet]
        [Route("{application}/{container}")]
        public IHttpActionResult GetContainer(string application, string container)
        {
            string resType = HttpContext.Current.Request.Headers["somiod-discover"];
            if (resType is null)
            {
                SqlConnection conn = null;
                SqlDataReader reader = null;
                try
                {
                    conn = new SqlConnection(connectionString);
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Containers WHERE Name = @Name AND Parent = (SELECT Id FROM Applications WHERE Name = @Application)", conn);
                    cmd.Parameters.AddWithValue("@Name", container);
                    cmd.Parameters.AddWithValue("@Application", application);

                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return Ok(new Container() { Id = (int)reader["Id"], Name = (string)reader["Name"], Parent = (int)reader["Parent"], CreationDT = (DateTime)reader["CreationDT"] });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
                finally
                {
                    reader?.Close();
                    conn?.Close();
                }
            }
            else if (resType == "subscription")
            {
                SqlConnection conn = null;
                SqlDataReader reader = null;
                try
                {
                    conn = new SqlConnection(connectionString);
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Subscriptions WHERE Parent = (SELECT Id FROM Containers WHERE Name = @Name AND Parent = (SELECT Id FROM Applications WHERE Name = @Application))", conn);
                    cmd.Parameters.AddWithValue("@Name", container);
                    cmd.Parameters.AddWithValue("@Application", application);

                    reader = cmd.ExecuteReader();
                    //List<Subscription> subscriptions = new List<Subscription>();
                    List<string> names = new List<string>();
                    while (reader.Read())
                    {
                        names.Add((string)reader["Name"]);
                        //subscriptions.Add(new Subscription() { Id = (int)reader["Id"], Name = (string)reader["Name"], Parent = (int)reader["Parent"], Event = ((string)reader["Event"])[0], Endpoint = (string)reader["Endpoint"], CreationDT = (DateTime)reader["CreationDT"] });
                    }
                    return Ok(names);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
                finally
                {
                    reader?.Close();
                    conn?.Close();
                }
            }
            else if (resType == "data")
            {
                SqlConnection conn = null;
                SqlDataReader reader = null;
                try
                {
                    conn = new SqlConnection(connectionString);
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Data WHERE Parent = (SELECT Id FROM Containers WHERE Name = @Name AND Parent = (SELECT Id FROM Applications WHERE Name = @Application))", conn);
                    cmd.Parameters.AddWithValue("@Name", container);
                    cmd.Parameters.AddWithValue("@Application", application);

                    reader = cmd.ExecuteReader();
                    //List<Data> data = new List<Data>();
                    List<string> names = new List<string>();
                    while (reader.Read())
                    {
                        names.Add((string)reader["Name"]);
                        //data.Add(new Data() { Id = (int)reader["Id"], Content = (string)reader["Content"], Parent = (int)reader["Parent"], CreationDT = (DateTime)reader["CreationDT"] });
                    }
                    return Ok(names);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
                finally
                {
                    reader?.Close();
                    conn?.Close();
                }
            }
            else
            {
                return BadRequest("Invalid somiod-discover header");
            }
        }


        /*
        GET http://<domain:port>/api/somiod/applicationX

        Return the application information or: 
            - if the header somiod-discover: container is present, return the containers of the application
        */
        [HttpGet]
        [Route("{application}")]
        public IHttpActionResult GetApplication(string application)
        {
            string resType = HttpContext.Current.Request.Headers["somiod-discover"];
            if (resType is null)
            {
                SqlConnection conn = null;
                SqlDataReader reader = null;
                try
                {
                    conn = new SqlConnection(connectionString);
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Applications WHERE Name = @Name", conn);
                    cmd.Parameters.AddWithValue("@Name", application);

                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return Ok(new Application() { Id = (int)reader["Id"], Name = (string)reader["Name"], CreationDT = (DateTime)reader["CreationDT"] });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
                finally
                {
                    reader?.Close();
                    conn?.Close();
                }
            }
            else if (resType == "container")
            {
                SqlConnection conn = null;
                SqlDataReader reader = null;
                try
                {
                    conn = new SqlConnection(connectionString);
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Containers WHERE Parent = (SELECT Id FROM Applications WHERE Name = @Name)", conn);
                    cmd.Parameters.AddWithValue("@Name", application);

                    reader = cmd.ExecuteReader();
                    //List<Container> containers = new List<Container>();
                    List<string> names = new List<string>();
                    while (reader.Read())
                    {
                        //containers.Add(new Container() { Id = (int)reader["Id"], Name = (string)reader["Name"], Parent = (int)reader["Parent"], CreationDT = (DateTime)reader["CreationDT"] });
                        names.Add((string)reader["Name"]);
                    }
                    return Ok(names);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
                finally
                {
                    reader?.Close();
                    conn?.Close();
                }
            }
            else
            {
                return BadRequest("Invalid somiod-discover header");
            }
        }

        /*
        POST http://<domain:port>/api/somiod/applicationX/containerY

        <resource res_type="subscription">
            <name>subscription1</name>
            <feature>PowerState</feature>
            <event>1</event>
            <endpoint>http://localhost:8080</endpoint>
        </resource>

        <resource res_type="data">
            <name>Data1</name>
            <content>
                <feature>PowerState</feature>
                <value>ON</value>
            </content>
        </resource>
        */
        [HttpPost]
        [Route("{application}/{container}")]
        public IHttpActionResult PostSubscriptionOrData(string application, string container)
        {
            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            string xml = bodyStream.ReadToEnd();

            XmlDocument doc = new XmlDocument();
            SqlConnection conn = null;
            SqlDataReader reader = null;
            try
            {
                doc.LoadXml(xml);
                XmlNode resTypeNode = doc.SelectSingleNode("/resource");

                if (resTypeNode is null)
                {
                    return BadRequest("Missing resource element");
                }

                XmlNode nameNode = resTypeNode.SelectSingleNode("name");
                string name = nameNode?.InnerText;

                string resType = resTypeNode.Attributes["res_type"]?.Value;
                if (resType == "subscription")
                {
                    XmlNode eventNode = resTypeNode.SelectSingleNode("event");
                    XmlNode endpointNode = resTypeNode.SelectSingleNode("endpoint");
                    XmlNode featureNode = resTypeNode.SelectSingleNode("feature");

                    if (eventNode is null || endpointNode is null || featureNode is null)
                    {
                        return BadRequest("Missing event, endpoint or feature element");
                    }

                    string feature = featureNode.InnerText;
                    string endpoint = endpointNode.InnerText;
                    char @event = eventNode.InnerText[0];

                    try
                    {
                        conn = new SqlConnection(connectionString);
                        conn.Open();

                        SqlCommand insertCmd;
                        if (name != null)
                        {
                            insertCmd = new SqlCommand("INSERT INTO Subscriptions (Name, Feature ,Parent, Event, Endpoint, CreationDT) OUTPUT INSERTED.ID, INSERTED.NAME, INSERTED.FEATURE, INSERTED.PARENT, INSERTED.EVENT, INSERTED.ENDPOINT VALUES (@Name, @Feature, (SELECT Id FROM Containers WHERE Name = @Container AND Parent = (SELECT Id FROM Applications WHERE Name = @Application)), @Event, @Endpoint, @CreationDT)", conn);
                            insertCmd.Parameters.AddWithValue("@Name", name);
                        }
                        else
                        {
                            insertCmd = new SqlCommand("INSERT INTO Subscriptions (Feature, Parent, Event, Endpoint, CreationDT) OUTPUT INSERTED.ID, INSERTED.NAME, INSERTED.FEATURE, INSERTED.PARENT, INSERTED.EVENT, INSERTED.ENDPOINT VALUES (@Feature, (SELECT Id FROM Containers WHERE Name = @Container AND Parent = (SELECT Id FROM Applications WHERE Name = @Application)), @Event, @Endpoint, @CreationDT)", conn);
                        }
                        insertCmd.Parameters.AddWithValue("@Container", container);
                        insertCmd.Parameters.AddWithValue("@Application", application);
                        insertCmd.Parameters.AddWithValue("@Event", @event);
                        DateTime creationDT = DateTime.Now;
                        insertCmd.Parameters.AddWithValue("@CreationDT", creationDT);
                        insertCmd.Parameters.AddWithValue("@Endpoint", endpoint);
                        insertCmd.Parameters.AddWithValue("@Feature", feature);

                        reader = insertCmd.ExecuteReader();
                        if (reader.Read())
                        {
                            int insertedId = (int)reader["Id"];
                            string insertedName = (string)reader["Name"];
                            int insertedParent = (int)reader["Parent"];
                            char insertedEvent = ((string)reader["Event"])[0];
                            string insertedEndpoint = (string)reader["Endpoint"];
                            string insertedFeature = (string)reader["Feature"];
                            return Ok(new Subscription() { Id = insertedId, Name = insertedName, Feature = insertedFeature, Parent = insertedParent, Event = insertedEvent, Endpoint = insertedEndpoint, CreationDT = creationDT });
                        }
                        else
                        {
                            return InternalServerError(new Exception("Error inserting subscription"));
                        }
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }
                }
                else if (resType == "data")
                {
                    XmlNode contentNode = resTypeNode.SelectSingleNode("content");

                    if (contentNode is null)
                    {
                        return BadRequest("Missing content element");
                    }

                    string feature = contentNode.SelectSingleNode("feature")?.InnerText;
                    string value = contentNode.SelectSingleNode("value")?.InnerText;

                    if (feature is null || value is null)
                    {
                        return BadRequest("Missing feature or value element");
                    }

                    try
                    {
                        conn = new SqlConnection(connectionString);
                        conn.Open();

                        SqlCommand insertCmd;
                        if (name != null)
                        {
                            insertCmd = new SqlCommand("INSERT INTO Data " +
                                "(Name, Content, Parent, CreationDT) OUTPUT INSERTED.ID, INSERTED.NAME, INSERTED.PARENT, INSERTED.CONTENT, INSERTED.CREATIONDT " +
                                "VALUES (@Name, @Content, (SELECT Id FROM Containers WHERE Name = @Container AND Parent = (SELECT Id FROM Applications WHERE Name = @Application)), @CreationDT)", conn);
                            insertCmd.Parameters.AddWithValue("@Name", name);
                        }
                        else
                        {
                            insertCmd = new SqlCommand("INSERT INTO Data " +
                                "(Content, Parent, CreationDT) OUTPUT INSERTED.ID, INSERTED.NAME, INSERTED.PARENT, INSERTED.CONTENT, INSERTED.CREATIONDT " +
                                "VALUES (@Content, (SELECT Id FROM Containers WHERE Name = @Container AND Parent = (SELECT Id FROM Applications WHERE Name = @Application)), @CreationDT)", conn);
                        }
                        string content = contentNode.OuterXml;
                        insertCmd.Parameters.AddWithValue("@Content", content);
                        insertCmd.Parameters.AddWithValue("@Container", container);
                        insertCmd.Parameters.AddWithValue("@Application", application);

                        DateTime creationDT = DateTime.Now;
                        insertCmd.Parameters.AddWithValue("@CreationDT", creationDT);

                        reader = insertCmd.ExecuteReader();
                        Data data = null;
                        if (reader.Read())
                        {
                            data = new Data
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                Parent = (int)reader["Parent"],
                                Content = (string)reader["Content"],
                                CreationDT = (DateTime)reader["CreationDT"]
                            };
                        }
                        else
                        {
                            return InternalServerError(new Exception("Error inserting data"));
                        }
                        reader.Close();

                        // Find the Subscriptions which are descendent of api/somiod/applicationX/containerY and which are interested in the event 0 - Both, 1 - Creation
                        SqlCommand selectSubscriptionsCmd = new SqlCommand("SELECT * FROM Subscriptions " +
                            "INNER JOIN Containers ON Subscriptions.Parent = Containers.Id " +
                            "INNER JOIN Applications ON Containers.Parent = Applications.Id " +
                            "WHERE Applications.Name = @Application AND Containers.Name = @Container AND Subscriptions.Event IN ('0', '1') AND Subscriptions.feature = @Feature", conn);

                        selectSubscriptionsCmd.Parameters.AddWithValue("@Application", application);
                        selectSubscriptionsCmd.Parameters.AddWithValue("@Container", container);
                        selectSubscriptionsCmd.Parameters.AddWithValue("@Feature", feature);

                        reader = selectSubscriptionsCmd.ExecuteReader();

                        // For each of them send a notification
                        string endpoint;
                        string xmlMessage = $"<notification><resource res_type='data'><name>{name}</name><content><feature>{feature}</feature><value>{value}</value></content></resource><event>1</event></notification>"; ;
                        while (reader.Read())
                        {
                            endpoint = (string)reader["Endpoint"];
                            SendNotification(application, container, feature, value, endpoint, xmlMessage);
                        }
                        return Ok(new Data() { Id = data.Id, Name = data.Name, Parent = data.Parent, Content = data.Content, CreationDT = creationDT });
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }
                }
                else
                {
                    return BadRequest("Invalid res_type attribute");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error processing XML: " + ex.Message);
            }
        }

        /*
        GET http://<domain:port>/api/somiod/applicationX/containerY/data/{name} 
        */
        [HttpGet]
        [Route("{application}/{container}/data/{name}")]
        public IHttpActionResult GetData(string application, string container, string name)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT Data.Id, Data.Name, Data.Parent, Data.Content, Data.CreationDT " +
                    "FROM Data " +
                    "INNER JOIN Containers ON Data.Parent = Containers.Id " +
                    "INNER JOIN Applications ON Containers.Parent = Applications.Id " +
                    "WHERE Data.Name = @Name AND Containers.Name = @Container AND Applications.Name = @Application", conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Container", container);
                cmd.Parameters.AddWithValue("@Application", application);

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Data data = new Data
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Parent = (int)reader["Parent"],
                        Content = (string)reader["Content"],
                        CreationDT = (DateTime)reader["CreationDT"]
                    };
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            finally
            {
                reader?.Close();
                conn?.Close();
            }
        }

        /*
        DELETE http://<domain:port>/api/somiod/applicationX/containerY/data/{name}
        */
        [HttpDelete]
        [Route("{application}/{container}/data/{name}")]
        public IHttpActionResult DeleteData(string application, string container, string name)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand selectCmd = new SqlCommand("SELECT Data.Id, Data.Name, Data.Parent, Data.Content, Data.CreationDT " +
                    "FROM Data " +
                    "INNER JOIN Containers ON Data.Parent = Containers.Id " +
                    "INNER JOIN Applications ON Containers.Parent = Applications.Id " +
                    "WHERE Data.Name = @Name AND Containers.Name = @Container AND Applications.Name = @Application", conn);
                selectCmd.Parameters.AddWithValue("@Name", name);
                selectCmd.Parameters.AddWithValue("@Container", container);
                selectCmd.Parameters.AddWithValue("@Application", application);

                reader = selectCmd.ExecuteReader();
                if (reader.Read())
                {
                    Data data = new Data
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Parent = (int)reader["Parent"],
                        Content = (string)reader["Content"],
                        CreationDT = (DateTime)reader["CreationDT"]
                    };

                    reader.Close();

                    XmlDocument doc = new XmlDocument();
                    XmlNode featureNode;
                    XmlNode valueNode;

                    doc.LoadXml(data.Content);
                    featureNode = doc.SelectSingleNode("/content/feature");
                    valueNode = doc.SelectSingleNode("/content/value");

                    // Find the Subscriptions which are children of that container
                    SqlCommand selectSubscriptionsCmd = new SqlCommand("SELECT * FROM Subscriptions " +
                        "INNER JOIN Containers ON Subscriptions.Parent = Containers.Id " +
                        "INNER JOIN Applications ON Containers.Parent = Applications.Id " +
                        "WHERE Applications.Name = @Application AND Containers.Name = @Container AND Subscriptions.Event IN ('0', '2') AND Subscriptions.feature = @Feature", conn);
                    selectSubscriptionsCmd.Parameters.AddWithValue("@Application", application);
                    selectSubscriptionsCmd.Parameters.AddWithValue("@Container", container);
                    selectSubscriptionsCmd.Parameters.AddWithValue("@Feature", featureNode.InnerText);
                    reader = selectSubscriptionsCmd.ExecuteReader();

                    string endpoint;
                    string xmlMessage = $"<notification><resource res_type=\"data\"><name>{name}</name><content><feature>{featureNode.InnerText}</feature><value>{valueNode.InnerText}</value></content></resource><event>2</event></notification>";
                    while (reader.Read())
                    {
                        endpoint = (string)reader["Endpoint"];
                        SendNotification(application, container, featureNode.InnerText, valueNode.InnerText, endpoint, xmlMessage);
                    }
                    reader.Close();

                    // Delete the Data
                    SqlCommand deleteCmd = new SqlCommand("DELETE FROM Data WHERE Id = @Id", conn);
                    deleteCmd.Parameters.AddWithValue("@Id", data.Id);
                    int rowsAffected = deleteCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok(data);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            finally
            {
                conn?.Close();
            }
        }

        /*
        GET http://<domain:port>/api/somiod/applicationX/containerY/sub/{name}
        */
        [HttpGet]
        [Route("{application}/{container}/sub/{name}")]
        public IHttpActionResult GetSubscription(string application, string container, string name)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT Subscriptions.Id, Subscriptions.Name, Subscriptions.Parent, Subscriptions.Event, Subscriptions.Endpoint, Subscriptions.CreationDT, Subscriptions.Feature " +
                    "FROM Subscriptions " +
                    "INNER JOIN Containers ON Subscriptions.Parent = Containers.Id " +
                    "INNER JOIN Applications ON Containers.Parent = Applications.Id " +
                    "WHERE Subscriptions.Name = @Name AND Containers.Name = @Container AND Applications.Name = @Application", conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Container", container);
                cmd.Parameters.AddWithValue("@Application", application);

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Subscription subscription = new Subscription
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Parent = (int)reader["Parent"],
                        Event = ((string)reader["Event"])[0],
                        Endpoint = (string)reader["Endpoint"],
                        CreationDT = (DateTime)reader["CreationDT"],
                        Feature = (string)reader["Feature"]
                    };
                    return Ok(subscription);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            finally
            {
                reader?.Close();
                conn?.Close();
            }
        }

        /*
        DELETE http://<domain:port>/api/somiod/applicationX/containerY/sub/{name}
        */
        [HttpDelete]
        [Route("{application}/{container}/sub/{name}")]
        public IHttpActionResult DeleteSubscription(string application, string container, string name)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand("DELETE Subscriptions " +
                    "FROM Subscriptions " +
                    "INNER JOIN Containers ON Subscriptions.Parent = Containers.Id " +
                    "INNER JOIN Applications ON Containers.Parent = Applications.Id " +
                    "WHERE Subscriptions.Name = @Name AND Containers.Name = @Container AND Applications.Name = @Application", conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Container", container);
                cmd.Parameters.AddWithValue("@Application", application);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            finally
            {
                conn?.Close();
            }
        }

        /*
        <notification>
            <resource res_type="data">
                <name>Data1</name>
                <content>
                    <feature>PowerState</feature>
                    <value>ON</value>
                </content>
            </resource>
            <event>1</event>
        </notification> 
        */
        private void SendNotification(string application, string container, string feature, string value, string endpoint, string xmlMessage)
        {
            if (endpoint.StartsWith("mqtt://"))
            {
                // Send MQTT message to the endpoint
                mcClient = new MqttClient(endpoint.Substring(7));
                mcClient.Connect(Guid.NewGuid().ToString());

                string channelName = "api/somiod/" + application + "/" + container;
                Console.WriteLine("Publishing on channel " + channelName);
                mcClient.Publish(channelName, Encoding.UTF8.GetBytes(xmlMessage));
            }
            else if (endpoint.StartsWith("http://"))
            {
                // Send POST request to the endpoint and the notification in the body
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{endpoint}/{application}/{container}");
                request.Method = "POST";
                request.ContentType = "application/xml";

                byte[] bytes = Encoding.UTF8.GetBytes(xmlMessage);
                request.ContentLength = bytes.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.Close();
            }
        }
    }
}
