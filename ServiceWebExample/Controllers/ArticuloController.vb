Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Web.Http
Imports Newtonsoft.Json

Namespace Controllers
    Public Class ArticuloController
        Inherits ApiController

        Public Function GetAll() As IHttpActionResult
            Dim response As HttpResponseMessage
            response = Request.CreateResponse(HttpStatusCode.NotFound)

            Try
                response = Request.CreateResponse(HttpStatusCode.OK)
                response.Content = New StringContent(JsonConvert.SerializeObject(New ArticuloRepository().GetAll()))
            Catch ex As Exception
                Dim errorMessage As ErrorMessage = New ErrorMessage()

                errorMessage.Message = ex.Message
                If (IsNothing(ex.InnerException) <> False) Then
                    errorMessage.InnerMessage = ex.InnerException
                End If
            End Try

            response.Content.Headers.ContentType = New MediaTypeHeaderValue("application/json")
            Return ResponseMessage(response)
        End Function

    End Class
End Namespace