﻿<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true" CodeBehind="AdminNews.aspx.cs" Inherits="UEB2.AdminNews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <title>UEB-News</title>
    <style>
        .Feedback {
            display: block;
            width: 800px;
            padding: 20px;
            margin: 30px auto;
            border-radius: 8px;
        }

        .btn {
            display: block;
            margin: auto;
        }

        #form1 {
            margin: auto;
        }

        .Name {
            display: block;
            margin: auto;
            width: 220px;
            border-radius: 5px;
            outline: none;
            margin-top: 20px;
            padding: 8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="title-heading-w3 text-center mx-auto">
        <h3 style="padding:20px;" class="title-big">News Updates</h3>
    </div>
    <form id="form1" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" AutoGenerateDeleteButton="true" DataKeyNames="Date" HorizontalAlign="Center" Width="100%" GridLines="None" Height="85px">
        <AlternatingRowStyle BackColor="White" ForeColor="#507CD1"/>
        <Columns>
            <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
            <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
            <asp:BoundField DataField="News" HeaderText="News" SortExpression="News" ItemStyle-Width="700px" />
        </Columns>
         <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
    
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:UEB1ConnectionString %>" SelectCommand="SELECT [Username], [Date], [Title], [News] FROM [tblNews]"
          DeleteCommand ="DELETE FROM [tblNews] WHERE [Date] = @Date">
          <DeleteParameters>
              <asp:Parameter Name="Date" />
          </DeleteParameters>
        </asp:SqlDataSource>
        <input type="text" id="Name" class="Name" runat="server" placeholder="Your Name" readonly/>
        <input type="text" id="NTitle" class="Name" runat="server" placeholder="News Title" required="" pattern="[A-Za-z0-9\s]{1,24}" title="Can enter 24 characters only"/>
        <textarea style="resize:none;" runat="server" placeholder="New news feed" class="Feedback" id="News" rows="5" cols="100" required=""></textarea>
        <input runat="server" type="submit" value="Submit" class="btn btn-primary btn-style" onserverclick="Submit"/>
    </form>
</asp:Content>
