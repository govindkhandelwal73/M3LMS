using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using M3LMS.ViewModels;

namespace M3LMS.Controllers
{
    public class UploadCourseController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return PartialView("Index");
        }


        [HttpPost]
        public ActionResult Index(FormCollection form, HttpPostedFileBase file)
        {

            //HttpPostedFileBase file = Request.Files[0 ];


            UploadsViewModel uploadsViewModel = Session["Uploads"] != null ? Session["Uploads"] as UploadsViewModel : new UploadsViewModel();


            uploadsViewModel.ID = long.Parse(form["id"]);

            File upload = new File();
            upload.FileID = uploadsViewModel.Uploads.Count + 1;
            upload.FileName = file.FileName;
            upload.FilePath = "~/TestFolder/" + file.FileName;



            //if (file.ContentLength < 4048576)    we can check file size before saving if we need to restrict file size or we can check it on client side as well
            //{

            if (file != null)
            {
                file.SaveAs(Server.MapPath(upload.FilePath));
                uploadsViewModel.Uploads.Add(upload);
                Session["Uploads"] = uploadsViewModel;
            }

            // Save FileName and Path to Database according to your business requirements

            //}


            return PartialView("~/Views/Shared/_UploadsPartial.cshtml", uploadsViewModel.Uploads);
        }


        public ActionResult DeleteFile(long id)
        {


            /* Use input Id to delete the record from db logically by setting IsDeleted bit in your table or delete it physically whatever is suitable for you
               for DEMO purpose i am stroing it in Session */

            UploadsViewModel viewModel = Session["Uploads"] as UploadsViewModel;

            File file = viewModel.Uploads.Single(x => x.FileID == id);

            try
            {

                System.IO.File.Delete(Server.MapPath(file.FilePath));
                viewModel.Uploads.Remove(file);

            }

            catch (Exception)
            {
                return PartialView("~/Views/Shared/_UploadsPartial.cshtml", viewModel.Uploads);
            }



            return PartialView("~/Views/Shared/_UploadsPartial.cshtml", viewModel.Uploads);
        }


        public ActionResult GetFiles(long Id)
        {
            UploadsViewModel viewModel = Session["Uploads"] as UploadsViewModel;

            return PartialView("~/Views/Shared/_UploadsPartial.cshtml", (viewModel == null ? new UploadsViewModel().Uploads : viewModel.Uploads));
        }


    }
}
