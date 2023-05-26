<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Invoice.aspx.cs" Inherits="NEA.Invoice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <script type ="text/javascript" src ="js/js.js"></script>

           <h3>Invoice Details</h3>

          <table>
              <tr> 
                  <td>Clients</td>
              </tr><tr>

              <td>
<asp:DropDownList ID="ddlClients" runat="server" DataSourceID="dsClients"
    DataTextField="Name" DataValueField="ClientId"
    OnSelectedIndexChanged="ddlClients_SelectedIndexChanged" AutoPostBack="true">
</asp:DropDownList>

<asp:SqlDataSource ID="dsClients" runat="server"
    ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
    SelectCommand="SELECT ClientId, Name, Gender, EmailAddress, MobileNo, HomeAddress FROM ClientsTbl WHERE UserId = @UserId">
    <SelectParameters>
        <asp:SessionParameter Name="UserId" SessionField="UserId" Type="Int32" DefaultValue="0" />
    </SelectParameters>
</asp:SqlDataSource>

               </td>
                  
              </tr>

              </table>
    <br /><table><tr>
                  <td>Name</td>
                    <td>Email</td>
        <td>Mobile</td>
        <td>Address</td>
        </tr>
        <tr>


            <td><asp:TextBox ID="txtfName" runat="server"></asp:TextBox>
                                <td><asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
                <td><asp:TextBox ID="txtMob" runat="server"></asp:TextBox></td>
            <td><asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
</td>
        
           
        </tr>
        


       </table><asp:GridView ID="gvServices" runat="server" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="Service" HeaderText="Service" />
        <asp:TemplateField HeaderText="Description">
            <ItemTemplate>
                <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>
      
        <asp:BoundField DataField="Rate" HeaderText="Rate" />
        <asp:TemplateField HeaderText="Quantity">
            <ItemTemplate>
                <asp:TextBox ID="txtQuantity" runat="server"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>--%>

    <table>
       <tr> <td>Services</td>













    <div></div>
    <p>Choose a service</p>
<asp:DropDownList ID="ddlService" runat="server" CssClass="form-control" SelectionMode="Multiple" OnSelectedIndexChanged="ddlService_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <div>

    </div>
    <table>
        <tr>
<td>            <asp:Label ID="Label1" runat="server" Text="Service"></asp:Label>
</td>   

            <td>
                <asp:Label ID="Label3" runat="server" Text="Description"></asp:Label>

            </td><td>            <asp:Label ID="Label2" runat="server" Text="Rate"></asp:Label>
</td>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Quantity"></asp:Label>
            </td>
        </tr>
  <tr>
        <td><asp:TextBox ID="txtService" runat="server"></asp:TextBox></td>
  
<td>    <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
</td>
          <td>  <asp:TextBox ID="txtRate" runat="server"></asp:TextBox> </td> 
<td>    <asp:TextBox ID="txtHours" runat="server"></asp:TextBox>
</td>  </tr>

</table>
    
    
    
    
    <div>    <asp:Button ID="btnAddService" runat="server" Text="Add service" OnClick="btnAddService_Click" />
  
    
    
    
    <div>
                  <asp:Button Text="Generate Invoice" OnClick="GenerateInvoicePDF" runat="server" />

  </div>

    
    </div>
</asp:Content>
