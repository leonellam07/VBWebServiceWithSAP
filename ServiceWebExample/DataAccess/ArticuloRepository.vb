Imports SAPbobsCOM

Public Class ArticuloRepository
    Public Function GetAll() As List(Of Articulo)
        Dim articulos As List(Of Articulo) = New List(Of Articulo)()

        Using db As ApplicationContext = New ApplicationContext()

            Dim query As String = "exec articulos"
            Dim recordset As Recordset = db.SBOCompany.GetBusinessObject(BoObjectTypes.BoRecordset)

            recordset.DoQuery(query)

            If (recordset.RecordCount = 0) Then
                Throw New Exception("No se encontraron registros")
            End If

            Dim idArticulo As String = String.Empty
            Dim articulo As Articulo
            Dim contador As Integer = 0

            While recordset.EoF = False

                If (contador = 0) Then
                    articulo = New Articulo
                    articulo.Id = recordset.Fields.Item("ItemCode").Value
                    articulo.Nombre = recordset.Fields.Item("ItemName").Value

                    articulos.Add(articulo)
                    idArticulo = articulo.Id

                End If

                Dim idArticuloNuevo = recordset.Fields.Item("ItemCode").Value
                If (idArticulo <> idArticuloNuevo) Then

                    articulo = New Articulo
                    articulo.Id = recordset.Fields.Item("ItemCode").Value
                    articulo.Nombre = recordset.Fields.Item("ItemName").Value

                    articulos.Add(articulo)
                    idArticulo = articulo.Id
                End If


                Dim almacen As Almacen = New Almacen With {
                    .Id = recordset.Fields.Item("WhsCode").Value,
                    .Nombre = recordset.Fields.Item("WhsName").Value,
                    .Stock = recordset.Fields.Item("OnHand").Value
                }

                articulo.Almacenes.Add(almacen)

                contador = contador + 1
                recordset.MoveNext()
            End While

        End Using

        Return articulos
    End Function
End Class
