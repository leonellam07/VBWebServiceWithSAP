﻿Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Web.Http
Imports Newtonsoft.Json

Namespace Controllers
    Public Class ViajeController
        Inherits ApiController

        <HttpPost>
        <Route("Taca/Viajes")>
        Public Function Add(<FromBody> ByVal viaje As Viaje) As IHttpActionResult
            Dim response As HttpResponseMessage
            response = Request.CreateResponse(HttpStatusCode.NotFound)

            Try
                response = Request.CreateResponse(HttpStatusCode.OK)
                response.Content = New StringContent(JsonConvert.SerializeObject(New ViajeRepository().Add(viaje)))
            Catch ex As Exception
                Dim errorMessage As ErrorMessage = New ErrorMessage()

                errorMessage.Message = ex.Message
                If (IsNothing(ex.InnerException) <> False) Then
                    errorMessage.InnerMessage = ex.InnerException
                End If

                response = Request.CreateResponse(HttpStatusCode.InternalServerError)
                response.Content = New StringContent(JsonConvert.SerializeObject(errorMessage))
            End Try


            response.Content.Headers.ContentType = New MediaTypeHeaderValue("application/json")
            Return ResponseMessage(response)
        End Function

        <HttpPut>
        <Route("Taca/Viajes")>
        Public Function Update(<FromBody> ByVal viaje As Viaje) As IHttpActionResult
            Dim response As HttpResponseMessage
            response = Request.CreateResponse(HttpStatusCode.NotFound)

            Try
                response = Request.CreateResponse(HttpStatusCode.OK)
                response.Content = New StringContent(JsonConvert.SerializeObject(New ViajeRepository().Update(viaje)))
            Catch ex As Exception
                Dim errorMessage As ErrorMessage = New ErrorMessage()

                errorMessage.Message = ex.Message
                If (IsNothing(ex.InnerException) <> False) Then
                    errorMessage.InnerMessage = ex.InnerException
                End If

                response = Request.CreateResponse(HttpStatusCode.InternalServerError)
                response.Content = New StringContent(JsonConvert.SerializeObject(errorMessage))
            End Try


            response.Content.Headers.ContentType = New MediaTypeHeaderValue("application/json")
            Return ResponseMessage(response)
        End Function

        <HttpDelete>
        <Route("Taca/Viajes")>
        Public Function Add(<FromUri> ByVal docEntry As Integer) As IHttpActionResult
            Dim response As HttpResponseMessage
            response = Request.CreateResponse(HttpStatusCode.NotFound)

            Try
                response = Request.CreateResponse(HttpStatusCode.OK)
                response.Content = New StringContent(JsonConvert.SerializeObject(New ViajeRepository().Delete(docEntry)))
            Catch ex As Exception
                Dim errorMessage As ErrorMessage = New ErrorMessage()

                errorMessage.Message = ex.Message
                If (IsNothing(ex.InnerException) <> False) Then
                    errorMessage.InnerMessage = ex.InnerException
                End If

                response = Request.CreateResponse(HttpStatusCode.InternalServerError)
                response.Content = New StringContent(JsonConvert.SerializeObject(errorMessage))
            End Try


            response.Content.Headers.ContentType = New MediaTypeHeaderValue("application/json")
            Return ResponseMessage(response)
        End Function
    End Class
End Namespace