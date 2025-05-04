using VirchowAspNetApi.Models;
using VirchowAspNetApi.Utils;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using Microsoft.Win32;
using System.Globalization;


namespace VirchowAspNetApi.Services;
public class LaudoPdfService
{
    public byte[] GerarPdf(Laudo laudo)
    {
        var documento = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);

                //Definição de margens
                page.MarginTop(100);
                page.MarginLeft(50);
                page.MarginRight(50);
                page.MarginBottom(50);

                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                page.Content().Column(col =>
                {
                    col.Spacing(5);
                    
                    // Nome, número do laudo e data do exame
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(inner =>
                        {
                            inner.Item().Text("Nome");
                            inner.Item().Text($"{laudo.NomePaciente}").Bold();
                        });

                        row.RelativeItem().AlignRight().Column(inner =>
                        {
                            inner.Item().Text($"Nº {(laudo.TipoLaudo?.Grupo ?? "")}");
                            inner.Item().Text($"{laudo.Id}").Bold();
                        });

                        row.RelativeItem().AlignRight().Column(inner =>
                        {
                            inner.Item().Text("Data do Exame");
                            string dataFormatada = laudo.DatExame.HasValue ? laudo.DatExame.Value.ToString("dd/MMM/yy", new CultureInfo("pt-BR")) : "";

                            inner.Item().Text($"{dataFormatada}");
                        });
                    });

                    // Idade e Estado civil
                    col.Item().Row(row =>
                    {

                        row.RelativeItem().Column(inner =>
                        {
                            inner.Item().Text($"Idade:        {laudo.Idade} anos");
                        });

                        row.RelativeItem().AlignRight().Column(inner =>
                        {
                            inner.Item().Text($"Estado Civil: {laudo.EstadoCivil}");
                        });
                    });

                    // Resumo Clínico
                    col.Item().Text("Resumo Clínico");
                    col.Item().Text(laudo.ResumoClinico);
                    
                    col.Item().PaddingTop(2).Text($"Médico Dr(a). {laudo.MedicoRequisitante}");
                    

                    // DESCRITIVO DO LAUDO

                    col.Item().PaddingTop(25).Element(container =>
                    {
                        container.RenderHtml(laudo.DesLaudo);
                    });


                });

                string dataImpessao = laudo.DatImpressao.HasValue ? laudo.DatImpressao.Value.ToString("dddd, dd 'de' MMMM 'de' yyyy", new CultureInfo("pt-BR")) : "";
                page.Footer()
                .AlignLeft()
                .Text(dataImpessao)
                .FontSize(10).FontColor(Colors.Grey.Darken2);
            });
        });

        return documento.GeneratePdf();
    }
}
