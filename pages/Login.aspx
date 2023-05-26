<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="NEA.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
   <table style="margin:auto;border: 5px solid white">
      <tr>
         <td>
            <asp:Label Text="Username" runat="server" />
         </td>
         <td colspan ="2" >
            <asp:TextBox ID ="txtLoginUsername" runat="server" />
         </td>
      </tr>
      <tr>
         <td>
            <asp:Label Text="Password" runat="server" />
         </td>
         <td colspan ="2" >
            <asp:TextBox ID ="txtLoginPassword" runat="server" TextMode="Password"/>
         </td>
      </tr>
      <tr>
         <td class="auto-style1"></td>
         <td colspan="2" class="auto-style1">
            <asp:Button Text ="Login" ID ="btnLogin" runat ="server" OnClick="btnLogin_Click" />
         </td>
      </tr>
      <tr>
         <td></td>
         <td>
         </td>
      </tr>
   </table>
</asp:Content>
