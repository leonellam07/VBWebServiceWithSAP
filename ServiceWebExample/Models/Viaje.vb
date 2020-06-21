Public Class Viaje
    Public Property Id As String
    Public Property Destino As String
    Public Property Pasaporte As String
    Public Property FechaCreacion As DateTime
    Public Property Detalles As List(Of ViajeDetalle) = New List(Of ViajeDetalle)()
    Public Property EsExtranjero As Boolean
End Class

Public Class ViajeDetalle
    Public Property Linea As Integer
    Public Property Ruta As String
    Public Property Precio As Double
        Public Property Eliminar As Boolean
End Class
