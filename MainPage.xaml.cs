using System.Globalization;

namespace CalificacionesUisrael;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        dpFecha.Date = DateTime.Today;
        ReiniciarResultados();
    }

    private async void OnCalcularClicked(object sender, EventArgs e)
    {
        if (pickerEstudiante.SelectedIndex == -1)
        {
            await DisplayAlert("Validación", "Debe seleccionar un estudiante.", "OK");
            return;
        }

        if (!ValidarNota(txtSeg1.Text, "Nota Seguimiento 1", out double seg1, out string error))
        {
            await DisplayAlert("Validación", error, "OK");
            return;
        }

        if (!ValidarNota(txtEx1.Text, "Examen 1", out double ex1, out error))
        {
            await DisplayAlert("Validación", error, "OK");
            return;
        }

        if (!ValidarNota(txtSeg2.Text, "Nota Seguimiento 2", out double seg2, out error))
        {
            await DisplayAlert("Validación", error, "OK");
            return;
        }

        if (!ValidarNota(txtEx2.Text, "Examen 2", out double ex2, out error))
        {
            await DisplayAlert("Validación", error, "OK");
            return;
        }

        double parcial1 = (seg1 * 0.3) + (ex1 * 0.2);
        double parcial2 = (seg2 * 0.3) + (ex2 * 0.2);
        double notaFinal = parcial1 + parcial2;

        string estado = ObtenerEstado(notaFinal);

        lblParcial1.Text = parcial1.ToString("0.00");
        lblParcial2.Text = parcial2.ToString("0.00");
        lblFinal.Text = notaFinal.ToString("0.00");
        lblEstado.Text = estado;
        ActualizarColorEstado(estado);

        string estudiante = pickerEstudiante.SelectedItem?.ToString() ?? "";

        DateTime fechaSeleccionada = dpFecha.Date ?? DateTime.Today;
        string fecha = fechaSeleccionada.ToString("dd/MM/yyyy");

        string mensaje =
            $"Nombre: {estudiante}\n" +
            $"Fecha: {fecha}\n" +
            $"Nota Parcial 1: {parcial1:0.00}\n" +
            $"Nota Parcial 2: {parcial2:0.00}\n" +
            $"Nota Final: {notaFinal:0.00}\n" +
            $"Estado: {estado}";

        await DisplayAlert("Resultado final", mensaje, "OK");
    }

    private bool ValidarNota(string? texto, string nombreCampo, out double valor, out string error)
    {
        valor = 0;
        error = string.Empty;

        if (string.IsNullOrWhiteSpace(texto))
        {
            error = $"Debe ingresar {nombreCampo}.";
            return false;
        }

        texto = texto.Trim().Replace(",", ".");

        bool esNumero = double.TryParse(
            texto,
            NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            out valor);

        if (!esNumero)
        {
            error = $"{nombreCampo} debe ser un valor numérico.";
            return false;
        }

        if (valor < 0 || valor > 10)
        {
            error = $"{nombreCampo} debe estar en el rango de 0 a 10.";
            return false;
        }

        return true;
    }

    private string ObtenerEstado(double notaFinal)
    {
        if (notaFinal >= 7)
            return "Aprobado";

        if (notaFinal >= 5 && notaFinal < 7)
            return "Complementario";

        return "Reprobado";
    }

    private void ActualizarColorEstado(string estado)
    {
        switch (estado)
        {
            case "Aprobado":
                lblEstado.TextColor = Colors.Green;
                lblFinal.TextColor = Colors.Green;
                break;

            case "Complementario":
                lblEstado.TextColor = Colors.Orange;
                lblFinal.TextColor = Colors.Orange;
                break;

            case "Reprobado":
                lblEstado.TextColor = Colors.Red;
                lblFinal.TextColor = Colors.Red;
                break;

            default:
                lblEstado.TextColor = Colors.Gray;
                lblFinal.TextColor = Colors.Black;
                break;
        }
    }

    private void OnLimpiarClicked(object sender, EventArgs e)
    {
        pickerEstudiante.SelectedIndex = -1;

        txtSeg1.Text = string.Empty;
        txtEx1.Text = string.Empty;
        txtSeg2.Text = string.Empty;
        txtEx2.Text = string.Empty;

        dpFecha.Date = DateTime.Today;

        ReiniciarResultados();
    }

    private void ReiniciarResultados()
    {
        lblParcial1.Text = "0.00";
        lblParcial2.Text = "0.00";
        lblFinal.Text = "0.00";
        lblEstado.Text = "Pendiente";

        ActualizarColorEstado("Pendiente");
    }
}