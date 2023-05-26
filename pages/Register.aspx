<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="NEA.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">   

   <asp:HiddenField ID ="hfUserID" runat="server" />
        <table>
      <tr>
         <td>
            <asp:Label Text="Username" runat="server" />
         </td>
         <td colspan="2">
            <asp:TextBox ID ="txtUsername" runat="server" />
            <asp:Label Text="*" runat="server" ForeColor="Red" />
         </td>
      </tr>
      <!-- username -->            
      <tr>
         <td>
            <asp:Label Text="Password" runat="server" />
         </td>
         <td colspan="2">
            <asp:TextBox ID ="txtPassword" runat="server" TextMode="Password" />
            <asp:Label Text="*" runat="server" ForeColor="Red" />
         </td>
      </tr>
      <!-- password -->
      <tr>
         <td>
            <asp:Label Text="Confirm Password" runat="server" />
         </td>
         <td colspan="2">
            <asp:TextBox ID ="txtConfirmPassword" runat="server" TextMode="Password" />
         </td>
      </tr>
      <!-- confirm password -->
      <tr>
         <td>
            <asp:Label Text="Email" runat="server" />
         </td>
         <td colspan="2">
            <asp:TextBox ID ="txtEmail" runat="server" />
         </td>
      </tr>
      <!-- email -->
            <tr>
         <td>
            <asp:Label Text="Type" runat="server" />
         </td>
         <td colspan="2">
             <asp:DropDownList ID="ddlType" runat="server">
                 <asp:ListItem>Junior</asp:ListItem>
                 <asp:ListItem>Senior</asp:ListItem>
                 <asp:ListItem></asp:ListItem>
             </asp:DropDownList>
         </td>
      </tr>
      <tr>
         <td class="auto-style1"></td>
         <td colspan="2" class="auto-style1">
            <asp:Button Text ="Submit" ID ="btnSubmit" runat ="server" OnClick="RegisterUser" Width="171px"  />
         </td>
      </tr>
      <tr>
         <td></td>
         <td colspan="2">
            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="GoToLogin">Already have an account? Login</asp:LinkButton>
         </td>
      </tr>
      
   </table>
       
    <div id="alert-wrapper"></div>
</asp:Content>
