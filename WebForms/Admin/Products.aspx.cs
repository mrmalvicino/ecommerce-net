﻿using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using DomainModelLayer;

namespace WebForms.Admin
{
    public partial class Products : BasePage
    {
        private ProductsManager _productsManager;

        private static Action<MasterPage> _modalOkAction;

        private int _temporalProductId;

        public Products()
        {
            _productsManager = new ProductsManager();
        }

        // METHODS

        protected string PrintCategoriesCount(object categoriesList)
        {
            var categories = categoriesList as List<Category>;

            if (categories == null || categories.Count < 2)
            {
                return "";
            }

            return $" (+{categories.Count - 1})";
        }

        private void BindProductList()
        {
            ProductListRepeater.DataSource = _productsManager.List();
            ProductListRepeater.DataBind();
        }

        private void CheckRequest()
        {
            if (string.IsNullOrEmpty(Request.QueryString["successDelete"]))
                return;
            if (Request.QueryString["successDelete"] == "true")
            {
                Notify("Producto eliminado con éxito!");
            }
        }

        private void Notify(string message)
        {
            Admin adminMP = (Admin)this.Master;
            adminMP.ShowMasterToast(message);
        }

        private void DeleteProductAction(MasterPage masterPage)
        {
            Product auxProduct = _productsManager.Read(_temporalProductId);
            _productsManager.Delete(auxProduct);
            ((Admin)masterPage).ShowMasterToast("Producto eliminado con éxito!");
            // TODO: Actualizar Repeater
        }

        public override void OnModalConfirmed()
        {
            if (_modalOkAction != null)
            {
                _modalOkAction(this.Master);
                _modalOkAction = null; // Limpiar luego de usar
            }
        }

        // EVENTS

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckRequest();
                BindProductList();
            }
        }

        protected void SearchBtn_Click(object sender, EventArgs e)
        {

        }

        protected void DeleteProductLnkBtn_Click(object sender, EventArgs e)
        {
            _temporalProductId = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            _modalOkAction = DeleteProductAction;

            Admin adminMP = (Admin)this.Master;
            adminMP.ShowMasterModal( // Llama y muestra el modal de la Masterpage
                "Eliminar Producto",
                "Está seguro que desea eliminar el producto?",
                true // requiere confirmación
            );
        }
    }
}
