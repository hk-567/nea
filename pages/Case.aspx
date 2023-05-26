<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Case.aspx.cs" Inherits="NEA.Case" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> <h1>Case Management</h1>
            <asp:GridView ID="gvCases" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="CaseId" HeaderText="ID" />
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="Type" HeaderText="Type" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="DateOpened" HeaderText="Opened" />
                </Columns>
            </asp:GridView><div></div>
   
    <table>
        <tr>
            <td>Name</td>
            <td>Type</td>
            <td>Status</td>
        </tr>
        <tr>
           <td><asp:TextBox ID="txtCaseName" runat="server" Width="101px"></asp:TextBox></td> 
          <td>   <asp:DropDownList ID="ddlCaseType" runat="server">
              <asp:ListItem>Civil</asp:ListItem>
              <asp:ListItem>Criminal</asp:ListItem>
              <asp:ListItem>Family</asp:ListItem>
            
              </asp:DropDownList></td> 
            <td> <asp:DropDownList ID="ddlStatus" runat="server">
                <asp:ListItem>Active</asp:ListItem>
                <asp:ListItem>Closed</asp:ListItem>
                <asp:ListItem>Pending</asp:ListItem>
                </asp:DropDownList></td> 
            <td>
                <asp:DropDownList ID="ddlCaseClient" runat="server">
                                       




                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [Name] FROM [ClientsTbl] WHERE ([UserId] = @UserId)">
                    <SelectParameters>
                        <asp:SessionParameter DefaultValue="-1" Name="UserId" SessionField="UserId" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
   </tr>
    </table>
    <asp:Button ID="btnCase" runat="server" Text="Add Case" OnClick="btnCase_Click" />
    <div></div>
    <asp:Label ID="scsslbl" runat="server" ></asp:Label>
</asp:Content>
