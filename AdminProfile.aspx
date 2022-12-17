﻿<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true" CodeBehind="AdminProfile.aspx.cs" Inherits="UEB2.AdminProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet">
    <title>UEB-AdminProfile</title>
    <style>
        body {
            background: url(/assets/images/AdminProfile.jpg);
            background-position: center,center;
            background-size: cover;
        }

        form {
            margin-bottom: 30px;
            margin:auto;
            width: 500px;
        }

        .headerP {
            font-size: 3rem;
            width: 300px;
            margin: 30px auto 40px auto;
            color: black;
            text-decoration: underline;
        }

        .form-group {

            margin: 10px auto;
            padding: 8px;
            margin:auto;
        }

            .form-group label {
                display: inline-block;
                width: 140px;
                font-size: 1.5rem;
                color: black;
                vertical-align: top;
                text-decoration:underline;
            }

            .form-group input {
                padding: 8px;
                border-radius: 5px;
                outline: none;
                width: 300px;
                border: none;
                border-bottom: 4px solid black;
            }

        .fa-pencil-square-o {
            color: black;
            cursor: pointer;
            vertical-align:unset;
            font-size:30px !important;
        }

        .btn{
            width:150px !important;
            margin: 20px auto;
            display: block;
            background-color:orangered;
        }

        .btn:hover{
            background-color:red;
        }
        .CustImage{
            margin: 0px 0px 20px 200px;
            border-radius: 50px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <h1 class="headerP">Your Profile</h1>
    <div class="form">
        <form runat="server" method="post">
            <img runat="server" id="CustImage" src="data:," alt="could not be found" height="100" width="100" class="CustImage" >
            <label class="fa fa-pencil-square-o fa-lg" ><input runat="server" type="file" id="ImageUpload" accept="image/*" style="display:none;"/></label>
            <div class="form-group">
                <label runat="server" for="Name">Name</label>
                <input runat="server" type="text" placeholder="Enter Your Name" id="Name" pattern="[A-Za-z\s]{1,24}" title ="Enter 24 characters only" required="" disabled>
                <a runat="server" onclick="EditN()"><i runat="server" class="fa fa-pencil-square-o fa-lg"></i></a>
            </div>

            <div class="form-group">
                <label runat="server" for="Mobile">Mobile No.</label>
                <input runat="server" type="text" placeholder="Enter Mobile No." id="Mobile" pattern="[0-9]{10}" title ="Enter 10 Digits only" required="" disabled>
                <a runat="server" onclick="EditM()"><i runat="server" class="fa fa-pencil-square-o fa-lg"></i></a>
            </div>
            <div class="form-group">
                <label runat="server" for="Email">Email ID</label>
                <input runat="server" type="text" placeholder="Enter your email id" id="Email" required="" disabled>
                <a runat="server" onclick="EditE()"><i runat="server" class="fa fa-pencil-square-o fa-lg"></i></a>
            </div>
            <div class="form-group">
                <label runat="server" for="Address">Address</label>
                <textarea style="border-bottom:4px solid black; border-radius:5px; resize:none;" runat="server" cols="28" type="text" placeholder="Enter your Address" id="Address" disabled  pattern = "[A-Za-z]{49}" title ="Enter 49 characters only" required=""></textarea>
                <a runat="server" onclick="EditA()"><i runat="server" class="fa fa-pencil-square-o fa-lg"></i></a>
            </div>
            <div class="form-group">
                <label runat="server" for="Aadhar">Aadhar No.</label>
                <input runat="server" type="text" placeholder="Enter your Aadhar Number" id="Aadhar" pattern="[0-9]{12}" title= "Enter 12 Digits only" required="" disabled>
            </div>
            <div class="form-group">
                <label runat="server" for="Username">Username</label>
                <input runat="server" type="text" placeholder="Enter Username" id="Username" pattern=".{3,20}" title="can enter only 3 to 20 characters" required="" disabled>
                <%--<a runat="server" onserverclick="EditU()"><i runat="server" class="fa fa-pencil-square-o fa-lg"></i></a>--%>
            </div>
            <div class="form-group">
                <label runat="server" for="Password">Password</label>
                <input runat="server" type="text" placeholder="Enter Password" id="Password" required="" disabled pattern="(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,24}" title="Must contain at least one  number and one uppercase and lowercase letter, and at least 8 and less than 24 characters">
                <a runat="server"  onclick="EditP()"><i runat="server" class="fa fa-pencil-square-o fa-lg"></i></a>
            </div> <%--onserverclick="EditP"--%>
            <div class="form-group">
                <input runat="server" type="submit" class="btn btn-primary" value="Update" onserverclick="Update" />
            </div>
        </form>
    </div>
    <script>
        document.getElementById("ContentPlaceHolder1_Password").type = "password";
        function EditP() {
            var x = document.getElementById("ContentPlaceHolder1_Password");
            if (x.disabled === true) {
                x.disabled = false;
            } else {
                x.disabled = true;
            }
            if (x.type === "password") {
                x.type = "text";
            } else {
                x.type = "password";
            }
        }

        function EditU() {
            var x = document.getElementById("ContentPlaceHolder1_Username");
            if (x.disabled == true) {
                x.disabled = false;
            } else {
                x.disabled = true;
            }
        }

        function EditA() {
            var x = document.getElementById("ContentPlaceHolder1_Address");
            if (x.disabled == true) {
                x.disabled = false;
            } else {
                x.disabled = true;
            }
        }

        function EditE() {
            var x = document.getElementById("ContentPlaceHolder1_Email");
            if (x.disabled == true) {
                x.disabled = false;
            } else {
                x.disabled = true;
            }
        }

        function EditM() {
            var x = document.getElementById("ContentPlaceHolder1_Mobile");
            if (x.disabled == true) {
                x.disabled = false;
            } else {
                x.disabled = true;
            }
        }

        function EditN() {
            var x = document.getElementById("ContentPlaceHolder1_Name");
            if (x.disabled == true) {
                x.disabled = false;
            } else {
                x.disabled = true;
            }
        }
    </script>
</asp:Content>
