Public Class Articulo
    Private _Id As String
    Private _Nombre As String

    Public Property Id As String
        Get
            Return _Id
        End Get
        Set
            If (IsNothing(Value) = False) Then
                _Id = Value
            End If
        End Set
    End Property

    Public Property Nombre As String
        Get
            Return _Nombre
        End Get
        Set
            If (IsNothing(Value) = False) Then
                _Nombre = Value
            End If
        End Set
    End Property

    Public Property Almacenes As List(Of Almacen) = New List(Of Almacen)
End Class

Public Class Almacen
    Private _Nombre As String
    Private _Stock As Double
    Public Property Id As String
    Public Property Nombre As String
        Get
            Return _Nombre
        End Get
        Set
            If (IsNothing(Value) = False) Then
                _Nombre = Value
            End If
        End Set
    End Property

    Public Property Stock As Double
        Get
            Return _Stock
        End Get
        Set
            If (IsNothing(Value) = False) Then
                _Stock = Value
            End If
        End Set
    End Property
End Class
