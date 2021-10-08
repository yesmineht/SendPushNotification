using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using SendNotificationProject.Models;
using System.IO;
using RestSharp;
using System.Net;
using System.Web.Script.Serialization;
using System.Text;
using System.Net.Http;

namespace SendNotificationProject.Controllers
{
    public class NotificationController : Controller
    {
        string connectionString = "data source=DESKTOP-BES7DQH;initial catalog=connectionDb;integrated security=True";
        [HttpGet]
        // GET: Notification
        public ActionResult Index()
        {
            DataTable tbNotif = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
              
                SqlDataAdapter sqlDa = new SqlDataAdapter("GetData", sqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(tbNotif);
            }
            return View(tbNotif);
        }



        [HttpGet]
        public ActionResult Create()
        {

            return View(new NotifModel());
        }

        // POST: Notification/Create
        [HttpPost]
        public ActionResult Create(NotifModel notifmodel, HttpPostedFileBase icon, HttpPostedFileBase image)

        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {

                    sqlCon.Open();
                    //String query = "Insert INTO WebPushNotification VALUES(@title,@text,@icon,@image,@clickAction) ";
                    SqlCommand sqlcmd = new SqlCommand("ManageData", sqlCon);
                    string msg = Request.Form.Get("text");
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@ID", notifmodel.ID);
                    sqlcmd.Parameters.AddWithValue("@title", notifmodel.title);
                    sqlcmd.Parameters.AddWithValue("@text", msg);

                    if (icon != null && icon.ContentLength > 0)
                    {
                        string filename = Path.GetFileName(icon.FileName);
                        string iconPath = Path.Combine(Server.MapPath("~/icon/") + filename);
                        icon.SaveAs(iconPath);

                        string iconpath_name = "~/icon/" + filename;
                        sqlcmd.Parameters.AddWithValue("@icon", @Url.Content(iconpath_name));


                    }

                    if (image != null && image.ContentLength > 0)
                    {
                        string filename_image = Path.GetFileName(image.FileName);
                        string imagePath = Path.Combine(Server.MapPath("~/image/") + filename_image);
                        image.SaveAs(imagePath);
                        string imagepath_name = "~/image/" + filename_image;
                        sqlcmd.Parameters.AddWithValue("@image", @Url.Content(imagepath_name));

                    }


                    sqlcmd.Parameters.AddWithValue("@clickAction", notifmodel.clickAction);
                    int i = sqlcmd.ExecuteNonQuery();
                    if (i >= 1)
                    {

                        ViewBag.Message = "New Notif Added Successfully";

                    }



                    return RedirectToAction("Create");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Create");
            }

        }
        // GET: Notification/Edit/5
        public ActionResult Edit(int id)
        {
            NotifModel notifModel = new NotifModel();
            DataTable tbNotif = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM WebPushNotification Where ID=@ID ", sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ID", id);
                sqlDa.Fill(tbNotif);
            }
            if (tbNotif.Rows.Count == 1)
            {
                notifModel.ID = Convert.ToInt32(tbNotif.Rows[0][0].ToString());
                notifModel.title = tbNotif.Rows[0][1].ToString();

                notifModel.text = tbNotif.Rows[0][2].ToString();
                notifModel.icon = tbNotif.Rows[0][3].ToString();
                notifModel.image = tbNotif.Rows[0][4].ToString();
                notifModel.clickAction = tbNotif.Rows[0][5].ToString();
                return View(notifModel);
            }
            else
                return RedirectToAction("Index");
        }

        // POST: Notification/Edit/5
        [HttpPost]
        public ActionResult Edit(NotifModel notifmodel, HttpPostedFileBase icon, HttpPostedFileBase image)
        {

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {

                    sqlCon.Open();
                    //String query = "Insert INTO WebPushNotification VALUES(@title,@text,@icon,@image,@clickAction) ";
                    SqlCommand sqlcmd = new SqlCommand("ManageData", sqlCon);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@ID", notifmodel.ID);
                    sqlcmd.Parameters.AddWithValue("@title", notifmodel.title);
                    string msg = Request.Form.Get("text");
                    sqlcmd.Parameters.AddWithValue("@text", msg);

                    if (icon != null && icon.ContentLength > 0)
                    {
                        string filename = Path.GetFileName(icon.FileName);
                        string iconPath = Path.Combine(Server.MapPath("~/icon/") + filename);
                        icon.SaveAs(iconPath);
                        string iconpath_name = "~/icon/" + filename;
                        sqlcmd.Parameters.AddWithValue("@icon", @Url.Content(iconpath_name));


                    }

                    if (image != null && image.ContentLength > 0)
                    {
                        string filename_image = Path.GetFileName(image.FileName);
                        string imagePath = Path.Combine(Server.MapPath("~/image/") + filename_image);
                        image.SaveAs(imagePath);
                        string imagepath_name = "~/image/" + filename_image;
                        sqlcmd.Parameters.AddWithValue("@image", @Url.Content(imagepath_name));
                    }


                    sqlcmd.Parameters.AddWithValue("@clickAction", notifmodel.clickAction);
                    int i = sqlcmd.ExecuteNonQuery();
                   



                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }





        // GET: Notification/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    //String query = "DELETE FROM  WebPushNotification  WHERE ID=@ID";
                    SqlCommand sqlcmd = new SqlCommand("DeleteData", sqlCon);

                       sqlcmd.CommandType = CommandType.StoredProcedure;
                    
                    sqlcmd.Parameters.AddWithValue("@ID", id);

                    sqlcmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }


            }
            catch (Exception ex)

            {
                return RedirectToAction("Index");
            }



        }



        public ActionResult DisplayToken(int id)
        {

            return PartialView("DisplayToken", id.ToString());

        }

        
        public void SendPush( string id, string token)

        {
            Push _Push = new Push();
            

                DataTable tbNotif = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
                try
                {
                    sqlCon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM WebPushNotification  Where ID=@ID ", sqlCon);
                    sqlDa.SelectCommand.Parameters.AddWithValue("@ID", id);
                    sqlDa.Fill(tbNotif);


                    objectnotification _objectnotification = new objectnotification();
                    _objectnotification.title = tbNotif.Rows[0][1].ToString();
                    _objectnotification.body = tbNotif.Rows[0][2].ToString();
                    _objectnotification.icon =  tbNotif.Rows[0][3].ToString();
                    _objectnotification.image =  tbNotif.Rows[0][4].ToString();
                    _objectnotification.click_action = tbNotif.Rows[0][5].ToString();

                    _Push.notification = _objectnotification;
                }
                catch (Exception ex)
                {

                }
            
            _Push.to = token;

            
           HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");

            request.Headers.Clear();

            request.Method = "POST";

            
            request.Headers.Add("Authorization", "key=" + "AAAAQfOtGrA:APA91bFRx1HN7Pmo3BGBX9AZHM6aHCZeYS_--70VuF7FKE7wz1UttgOvEHDi6wEbjKB6Gdrn85AqFMBkfAt_xsDmrEnq18wSu6x0-dGE2KUUzY-KobQOOCV8FvVUDAglxWod3rUi3Ehb");

            request.ContentType = "application/json";

            request.UseDefaultCredentials = true;

            request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;



            // turn our request string into a byte stream

            string para = new JavaScriptSerializer().Serialize(_Push);

            byte[] postBytes = Encoding.UTF8.GetBytes(para);

            request.ContentLength = postBytes.Length;

            Stream requestStream = request.GetRequestStream();

            // now send it




            requestStream.Write(postBytes, 0, postBytes.Length);

            requestStream.Close();


            // grab te response and print it out to the console along with the status code

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
           



            using (StreamReader rdr = new StreamReader(response.GetResponseStream()))

                {

                   string Response = rdr.ReadToEnd();

                }

            


        }

        }
           
         }

        


     
 
    









//}

