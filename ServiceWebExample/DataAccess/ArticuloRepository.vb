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
            Dim articulo As Articulo = New Articulo
            Dim primerRegistro As Boolean = True

            While recordset.EoF = False

                If (primerRegistro = True) Then
                    articulo.Id = recordset.Fields.Item("ItemCode").Value
                    articulo.Nombre = recordset.Fields.Item("ItemName").Value
                    articulos.Add(articulo)

                    idArticulo = articulo.Id
                    primerRegistro = False
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
                recordset.MoveNext()
            End While

        End Using

        Return articulos
    End Function

    Public Function Add(ByVal articulo As Articulo) As Articulo
        Using db As ApplicationContext = New ApplicationContext()

            Dim nuevoArticulo As Items = CType(db.SBOCompany.GetBusinessObject(BoObjectTypes.oItems), Items)

            If (db.SBOCompany.InTransaction) Then
                db.SBOCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If

            db.SBOCompany.StartTransaction()

            nuevoArticulo.ItemName = articulo.Nombre
            nuevoArticulo.ItemType = ItemTypeEnum.itItems
            nuevoArticulo.Series = 71 'Para repuestos
            nuevoArticulo.ItemsGroupCode = 110 'Para Inventario

            If (nuevoArticulo.Add() <> 0) Then
                Throw New Exception(db.GetErrorLog())
            End If

            db.SBOCompany.EndTransaction(BoWfTransOpt.wf_Commit)
            articulo.Id = db.SBOCompany.GetNewObjectKey()

            Return articulo
        End Using
    End Function
End Class
