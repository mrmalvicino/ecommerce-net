﻿<%@ Page Title="Pedidos" Language="C#" MasterPageFile="AdminMP.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="WebForms.Admin.Orders" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <div class="d-flex flex-column container-800 mx-auto gap-3">

        <!-- Título -->

        <div class="d-flex align-items-center justify-content-between">
            <h1 class="fs-4 m-0">Pedidos</h1>
        </div>

        <!-- Buscador -->

        <asp:Panel ID="SearchPanel" runat="server" CssClass="input-group mb-3" DefaultButton="searchBtn">
            <asp:TextBox
                CssClass="form-control"
                ID="SearchTextBox"
                runat="server"
                Text=""
                onKeyUp="checkSearchBtn();"
                placeholder="Buscar por nombre de usuario o cliente" />
            <asp:LinkButton
                Text='<i class="bi bi-search"></i>'
                ID="SearchBtn"
                CssClass="btn rounded-end btn-outline-secondary"
                runat="server"
                ClientIDMode="Static"
                OnClick="SearchBtn_Click" />
            <div class="invalid-feedback">
                Ingrese al menos 2 caracteres.
            </div>
        </asp:Panel>

        <!-- Listado -->

        <div class="table-responsive card">
            <table class="table table-hover">
                <thead class="table-light">
                    <tr>
                        <th scope="col">Fecha</th>
                        <th scope="col">Cliente</th>
                        <th scope="col">Distribución</th>
                        <th scope="col">Estado</th>
                        <th scope="col">Acciones</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="OrdersListRpt" OnItemCommand="OrdersListRpt_ItemCommand">
                        <ItemTemplate>
                            <tr>

                                <!-- Fecha (de creación) -->

                                <td scope="row">
                                    <asp:Label
                                        ID="DateLbl"
                                        runat="server"
                                        Text='<%#Eval("CreationDate", "{0:dd/MM/yyyy}")%>'
                                        CssClass="text-black">
                                    </asp:Label>
                                </td>

                                <!-- Cliente -->

                                <td>
                                    <asp:Label
                                        ID="CategoryNameLbl"
                                        runat="server"
                                        Text='<%# Eval("User")?.ToString() ?? "N/A" %>'
                                        CssClass="text-black">
                                    </asp:Label>
                                </td>

                                <!-- Canal de distribución -->

                                <td>
                                    <asp:Label
                                        ID="DistributionChannelLbl"
                                        runat="server"
                                        Text='<%#Eval("DistributionChannel")%>'
                                        CssClass="text-black">
                                    </asp:Label>
                                </td>

                                <!-- Estado -->

                                <td>
                                    <asp:Label
                                        ID="OrderStatusLbl"
                                        runat="server"
                                        Text='<%#Eval("OrderStatus")%>'
                                        CssClass="text-black">
                                    </asp:Label>
                                </td>

                                <!-- Acciones -->

                                <td>
                                    <div class="d-flex gap-2">

                                        <!-- Editar -->

                                        <asp:LinkButton
                                            Text='<i class="bi bi-pencil-square"></i>'
                                            CssClass="p-0 text-black fs-5"
                                            CommandName="Edit"
                                            CommandArgument='<%#Eval("Id")%>'
                                            ID="EditOrderBtn"
                                            runat="server" />

                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
