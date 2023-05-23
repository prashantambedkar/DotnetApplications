using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.IO;
using System.Data.SqlClient;

public partial class usercontrol_ProductDetailsCS : System.Web.UI.UserControl
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    public string ImageName
    {
        get
        {
            if (ViewState["ImageName"] == null)
            {
                return "";
            }
            return (string)ViewState["ImageName"];
        }
        set
        {

            ViewState["ImageName"] = value;

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void OnPreRender(EventArgs e)
    {
        try
        {
            base.OnPreRender(e);
            string Query = @"if exists(Select  *  from AssetMasterIdentifiedAndroid where ImageName=('" + this.ImageName + "'))Begin Select  *  from AssetMasterIdentifiedAndroid where ImageName=('" + this.ImageName + "') end else Begin if exists(Select  *  from AssetMaster where ImageName=('" + this.ImageName + "'))"
                + @" begin Select * from AssetMaster where ImageName = ('" + this.ImageName + "') end else begin Select  top 1 '1' as AssetId,'amswatermark.png' as ImageName,'' as Category  from AssetMaster end end";
            SqlDataAdapter da = new SqlDataAdapter(Query, con);
            da.SelectCommand.CommandTimeout = 3000;
            DataSet ds = new DataSet();
            da.Fill(ds);
            ImageView.DataSource = ds;
            ImageView.DataBind();
        }
        catch (Exception ex)
        {

        }
        //this.ProductDataSource.SelectParameters["ImageName"].DefaultValue = this.ImageName;
        //this.DataBind();
    }


    protected void ImageView_DataBound(object sender, EventArgs e)
    {
        try
        {
            System.Web.UI.WebControls.Image image = (System.Web.UI.WebControls.Image)ImageView.FindControl("image");
            if (image == null)
            {
                image.ImageUrl = "../images/AMSWatermark.png";
                return;
            }
            if (!File.Exists(MapPath(image.ImageUrl)))
            {
                image.ImageUrl = "../images/AMSWatermark.png";
            }
        }
        catch (Exception ex)
        {

        }
    }
}