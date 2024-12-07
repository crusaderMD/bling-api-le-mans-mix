using BlingApiDailyConsult.Entities.XMLEntities;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BlingApiDailyConsult.Infrastructure
{
    internal class BlingXMLFetcher
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<NfeProc?> GetXML(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Erro ao obter o XML. StatusCode: {response.StatusCode}");
                }

                string xmlContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(xmlContent))
                {
                    throw new InvalidDataException("O conteúdo do XML está vazio ou inválido.");
                }

                XmlSerializer serializer = new XmlSerializer(typeof(NfeProc));
                using (StringReader sr = new StringReader(xmlContent))
                {
                    return (NfeProc?)serializer.Deserialize(sr);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro na requisição HTTP: {ex.Message}");
                throw;
            }
            catch (XmlException ex)
            {
                Console.WriteLine($"Erro ao deserializar o XML: {ex.Message}");
                throw;
            }
        }
    }
}
