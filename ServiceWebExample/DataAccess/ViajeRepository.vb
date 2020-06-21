Imports SAPbobsCOM

Public Class ViajeRepository
    Public Function Add(ByVal viaje As Viaje) As Viaje
        Using db As ApplicationContext = New ApplicationContext()
            If (db.SBOCompany.InTransaction) Then
                db.SBOCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If

            db.SBOCompany.StartTransaction()

            Dim udo As GeneralService = db.SBOCompany.GetCompanyService().GetGeneralService("VIAJES")   'Buscar el UDO

            Dim data As GeneralData = CType(udo.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData), GeneralData) 'Encontrar los campos de Usuario

            'Encabezado
            data.SetProperty("U_Destino", viaje.Destino)
            data.SetProperty("U_Pasaporte", viaje.Pasaporte)
            If (viaje.EsExtranjero) Then
                data.SetProperty("U_EsExtranjero", "Y")
            Else
                data.SetProperty("U_EsExtranjero", "N")
            End If

            'Detalle                
            Dim detalleColleccion As GeneralDataCollection = data.Child("DOCUMENTO_FILAS")

            For Each detalle As ViajeDetalle In viaje.Detalles
                Dim dataDetalle As GeneralData = detalleColleccion.Add()
                dataDetalle.SetProperty("U_Ruta", detalle.Ruta)
                dataDetalle.SetProperty("U_Precio", detalle.Precio)
            Next

            'Guardado
            Dim udoParams As GeneralDataParams = udo.Add(data)

            If (db.SBOCompany.InTransaction) Then
                db.SBOCompany.EndTransaction(BoWfTransOpt.wf_Commit)

                viaje.Id = udoParams.GetProperty("DocEntry")
                viaje.FechaCreacion = DateTime.Now
            End If

        End Using

        Return viaje
    End Function


    Public Function Update(ByVal viaje As Viaje) As Viaje
        Using db As ApplicationContext = New ApplicationContext()
            If (db.SBOCompany.InTransaction) Then
                db.SBOCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If

            db.SBOCompany.StartTransaction()

            Dim udo As GeneralService = db.SBOCompany.GetCompanyService().GetGeneralService("VIAJES")   'Buscar el UDO

            Dim udoParams As GeneralDataParams = udo.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams) 'Obtener parametros
            udoParams.SetProperty("DocEntry", Convert.ToInt32(viaje.Id))

            Dim data As GeneralData = udo.GetByParams(udoParams)

            'Encabezado
            data.SetProperty("U_Destino", viaje.Destino)
            data.SetProperty("U_Pasaporte", viaje.Pasaporte)
            If (viaje.EsExtranjero) Then
                data.SetProperty("U_EsExtranjero", "Y")
            Else
                data.SetProperty("U_EsExtranjero", "N")
            End If

            'Detalle                
            Dim detalleColleccion As GeneralDataCollection = data.Child("DOCUMENTO_FILAS")

            For Each detalle As ViajeDetalle In viaje.Detalles
                If (detalle.Eliminar) Then
                    detalleColleccion.Remove(detalle.Linea)
                Else
                    Dim dataDetalle As GeneralData = detalleColleccion.Add()
                    dataDetalle.SetProperty("U_Ruta", detalle.Ruta)
                    dataDetalle.SetProperty("U_Precio", detalle.Precio)
                End If

            Next

            udo.Update(data)

            If (db.SBOCompany.InTransaction) Then
                db.SBOCompany.EndTransaction(BoWfTransOpt.wf_Commit)
            End If

            Return viaje
        End Using
    End Function

    Public Function Delete(ByVal numeroEntrada As Integer) As String
        Using db As ApplicationContext = New ApplicationContext()
            If (db.SBOCompany.InTransaction) Then
                db.SBOCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If

            db.SBOCompany.StartTransaction()

            Dim udo As GeneralService = db.SBOCompany.GetCompanyService().GetGeneralService("VIAJES")   'Buscar el UDO

            Dim udoParams As GeneralDataParams = udo.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            udoParams.SetProperty("DocEntry", numeroEntrada)

            udo.Delete(udoParams)

            If (db.SBOCompany.InTransaction) Then
                db.SBOCompany.EndTransaction(BoWfTransOpt.wf_Commit)
            End If

            Return "Eliminado"
        End Using
    End Function
End Class
