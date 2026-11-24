using Kobra.Entidades;

namespace Kobra.UI
{
    /// <summary>Elemento de un combo de clientes: "CÓDIGO - Nombre" con el cliente completo a mano.</summary>
    internal sealed record ItemCliente(int Id, string Texto, Cliente? Cliente = null)
    {
        /// <summary>
        /// Llena el combo con los clientes y conserva la selección anterior. <paramref name="opcionVacia"/>
        /// agrega una primera opción sin cliente (por ejemplo "Todos los clientes").
        /// </summary>
        public static void Llenar(ComboBox combo, IEnumerable<Cliente> clientes, string? opcionVacia = null)
        {
            var seleccionado = (combo.SelectedItem as ItemCliente)?.Id ?? 0;

            var items = new List<ItemCliente>();
            if (opcionVacia is not null)
                items.Add(new ItemCliente(0, opcionVacia));
            items.AddRange(clientes.Select(c => new ItemCliente(c.Id, $"{c.Codigo} - {c.Nombre}", c)));

            combo.DataSource = items;
            combo.DisplayMember = nameof(Texto);
            combo.ValueMember = nameof(Id);
            if (seleccionado != 0)
                combo.SelectedValue = seleccionado;
        }
    }
}
