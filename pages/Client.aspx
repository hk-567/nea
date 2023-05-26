<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Client.aspx.cs" Inherits="NEA.Client" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <style type="text/css">
      .auto-style1 {
      width: 168px;
      height: 27px;
      }
   </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div>
      <h2>My Clients
      </h2>
      <asp:PlaceHolder ID = "PlaceHolder1" runat="server" />
   </div>
   <div>
      <h2>Add a new client</h2>
      <table>
         <tr>
            <td style="width:168px">
               <asp:Label Text="First name" runat="server" />
            </td>
            <td style="width:168px">
               <asp:Label Text="Middle Name" runat="server" />
            </td>
            <td style="width:168px">
               <asp:Label Text="Last Name" runat="server"/>
            </td>
         </tr>
         <tr>
            <td >
               <asp:TextBox ID ="txtfName" runat="server" />
            </td>
            <td >
               <asp:TextBox ID ="txtmName" runat="server" />
               <%-- <asp:Label Text="*" runat="server" ForeColor="Red" />--%>
            </td>
            <td>
               <asp:TextBox ID ="txtlName" runat="server" />
            </td>
         </tr>
         <tr>
            <td style="width:168px">
               <asp:Label Text="Gender" runat="server" />
            </td>
            <td style="width:168px">
               <asp:Label Text="Email Address" runat="server" />
            </td>
            <td style="width:168px">
               <asp:Label Text="Mobile No" runat="server" />
            </td>
         </tr>
         <tr>
            <td>
               <asp:RadioButtonList ID="rdGender" runat="server" Width="153px" RepeatDirection="Horizontal">
                  <asp:ListItem>Male</asp:ListItem>
                  <asp:ListItem>Female</asp:ListItem>
               </asp:RadioButtonList>
            </td>
            <td>
               <asp:TextBox ID ="txtEmail" runat="server" TextMode="Email"/>
            </td>
            <td>
               <asp:TextBox ID ="txtMob" runat="server" TextMode="Phone"/>
            </td>
         </tr>
      </table>
      <table>
         <tr>
            <td class="auto-style1">
               <asp:Label Text="Home Address" runat="server" />
            </td>
         </tr>
         <tr>
            <td>
               <asp:TextBox ID ="txtAddress" runat="server" TextMode="MultiLine" />
            </td>
            <td>
               <asp:Button ID="txtAddClients" runat="server" Text="Add new client" OnClick="txtAddClients_Click" />
            </td>
         </tr>
      </table>
      <asp:Label ID="successLbl" runat="server" ></asp:Label>
   </div>
</asp:Content>
