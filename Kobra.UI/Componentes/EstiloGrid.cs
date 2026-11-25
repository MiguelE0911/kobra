namespace Kobra.UI
{
    /// <summary>Apariencia común de las rejillas de solo lectura de todos los módulos.</summary>
    internal static class EstiloGrid
    {
        public static void Aplicar(DataGridView g)
        {
            g.Dock = DockStyle.Fill;
            g.ReadOnly = true;
            g.AutoGenerateColumns = false;
            g.AllowUserToAddRows = false;
            g.AllowUserToDeleteRows = false;
            g.AllowUserToResizeRows = false;
            g.RowHeadersVisible = false;
            g.MultiSelect = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.BorderStyle = BorderStyle.None;
            g.BackgroundColor = Tema.Blanco;
            g.GridColor = Tema.ContenedorAlto;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            g.ColumnHeadersHeight = g.LogicalToDeviceUnits(28);
            g.RowTemplate.Height = g.LogicalToDeviceUnits(24);
            g.ColumnHeadersDefaultCellStyle.BackColor = Tema.Primario;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Tema.SobrePrimario;
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Tema.Primario;
            g.ColumnHeadersDefaultCellStyle.Font = Tema.Negrita(9F);
            g.DefaultCellStyle.BackColor = Tema.Blanco;
            g.DefaultCellStyle.ForeColor = Tema.Texto;
            g.DefaultCellStyle.SelectionBackColor = Tema.Contenedor;
            g.DefaultCellStyle.SelectionForeColor = Tema.Texto;
            g.AlternatingRowsDefaultCellStyle.BackColor = Tema.ContenedorBajo;
        }

        /// <summary>
        /// Objeto enlazado a la fila seleccionada. Se lee de <c>SelectedRows</c> y, si no hay, de <c>CurrentRow</c>:
        /// según cómo se cambie la fila (clic, teclado o código) solo uno de los dos llega ya actualizado.
        /// </summary>
        public static T? Seleccionado<T>(DataGridView g) where T : class
        {
            var fila = g.SelectedRows.Count > 0 ? g.SelectedRows[0] : g.CurrentRow;
            return fila?.DataBoundItem as T;
        }

        /// <summary>
        /// Agrega una columna de texto. <paramref name="propiedad"/> enlaza la columna a una propiedad del
        /// origen de datos; sin ella, la columna se llena a mano.
        /// </summary>
        public static DataGridViewTextBoxColumn AgregarColumna(DataGridView g, string nombre, string encabezado,
            float peso, bool derecha = false, string? formato = null, bool negrita = false, string? propiedad = null)
        {
            var col = new DataGridViewTextBoxColumn
            {
                Name = nombre,
                HeaderText = encabezado,
                DataPropertyName = propiedad ?? string.Empty,
                FillWeight = peso,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            if (derecha)
            {
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (formato is not null)
                col.DefaultCellStyle.Format = formato;
            if (negrita)
                col.DefaultCellStyle.Font = Tema.Negrita(9F);
            g.Columns.Add(col);
            return col;
        }
    }
}
