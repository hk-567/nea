using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace NEA
{
    public partial class File : System.Web.UI.Page {
        private readonly FileRepository fileRepository;
   public File()
        {
    // Gets the connection string from the configuration file and create a new instance of the file repository
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            this.fileRepository = new FileRepository(connectionString);
        }

 protected void Page_Load(object sender, EventArgs e)
        {
            // Binds the grid with files on the page load
            if (!IsPostBack)// method is called only if the page is not being reloaded to improve performance and reduce network traffic
            {
                BindGrid();
            }
        }

 private void BindGrid()
{
    try
    {        // Get all files from the file repository


        var files = this.fileRepository.GetAllFiles();

        if (files != null && files.Count > 0)
        {            // If there are files, bind the grid to the data and display it


            gvFiles.DataSource = files;
            gvFiles.DataBind();
        }
        else
        {
            // No files found
            gvFiles.DataSource = null;
            gvFiles.DataBind();
             MessageHelper.ShowErrorMessage(this.Page, "No files found.");
        }
    }
    catch (Exception ex)
    {
       

        // Display a friendly error message
       MessageHelper.ShowErrorMessage(this.Page, "An error occurred while binding the grid with files. Please try again later.");
    }
}

protected void Upload(object sender, EventArgs e)
{
    try
    {
        // Gets the file name and content type from the uploaded file
        string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
        string contentType = FileUpload1.PostedFile.ContentType;

        // Validate the uploaded file
        if (!IsFileValid(contentType))
        {
            MessageHelper.ShowErrorMessage(this.Page,"Invalid file type. Please upload a valid file.");
            return;
        }

        if (!IsFileSizeValid(FileUpload1.PostedFile.ContentLength))
        {
            MessageHelper.ShowErrorMessage(this.Page,"File size must be less than 10 MB.");
            return;
        }

        // Reads the uploaded file into a byte array
        using (Stream fs = FileUpload1.PostedFile.InputStream)
        {
            using (BinaryReader br = new BinaryReader(fs))
            {
                byte[] bytes = br.ReadBytes((Int32)fs.Length);

                // Perform file minimization algorithm here
                byte[] minimizedBytes = MinimizeFile(bytes);

                // Create a new file model object and assign the uploaded file data to it
                var file = new FileModel
                {
                    Name = filename,
                    ContentType = contentType,
                    Data = minimizedBytes
                };

                // Adding file details to database
                using (var fileRepo = new FileRepository(connectionString))
                {
                    fileRepo.InsertFile(file);
                }
            }
        } 

        // Redirect to the current page to update the gridview with the newly uploaded file
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    catch (Exception ex)
    {
        MessageHelper.ShowErrorMessage(this.Page,"An unexpected error occurred. Please try again later.");
    }

    // Call the MinimizeFile method here to minimise the uploaded file
    MinimizeFile(minimizedBytes);
}

protected void DownloadFile(object sender, EventArgs e)
{
    try
    {
        // gets the file ID from the LinkButton command argument
        int id = int.Parse((sender as LinkButton).CommandArgument);

        // Verify that the file ID is valid
        var file = this.fileRepository.GetFileById(id);

        if (file != null)
        {
            // set up the HTTP response to download the file
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = file.ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.BinaryWrite(file.Data);
            Response.Flush();
            Response.End();
        }
        else
        {
            // Display an error message if the file is null
            MessageHelper.ShowErrorMessage(this.Page,"File not found.");
        }
    }
    catch (Exception ex)
    {
            MessageHelper.ShowErrorMessage(this.Page,"An unexpected error occurred.");
    }
}
private byte[] MinimizeFile(byte[] bytes)
{
    // Perform file minimization algorithm here
    // For example, you could remove all occurrences of a specific character
    char charToRemove = 'a';
    string str = Encoding.Default.GetString(bytes); // converting it to a string, replacing the character, and then converting it back to a byte array
    str = str.Replace(charToRemove.ToString(), "");
    return Encoding.Default.GetBytes(str);
}

private bool IsFileValid(string contentType)
{
    string[] validTypes = { "image/jpeg", "image/png", "application/pdf", "application/msword", "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };
    // Return true if the given content type is in the list of valid types, false otherwise


    return validTypes.Contains(contentType);
}

private bool IsFileSizeValid(int contentLength)
{    // Maximum file size allowed in bytes
    int maxSize = 10 * 1024 * 1024; // 10 MB
    return contentLength <= maxSize;
}



        }
    }

