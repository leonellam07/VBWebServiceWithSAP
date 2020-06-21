Public Class Viaje
    Public Property Id As String
    Public Property Destino As String
    Public Property Pasaporte As String
    Public Property FechaCreacion As DateTime
    Public Property Detalles As List(Of ViajeDetalle) = New List(Of ViajeDetalle)()

    Private _EsExtranjero As Boolean
    Public Sub EsExtranjero(ByVal value As Boolean)
        _EsExtranjero = value
    End Sub
    Public Function EsExtranjero()
        If (_EsExtranjero) Then
            Return "Y"
        End If
        Return "N"
    End Function

End Class

Public Class ViajeDetalle
    Public Property Ruta As String
    Public Property Precio As Double
End Class
