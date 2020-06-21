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
            data.SetProperty("U_EsExtranjero", viaje.EsExtranjero)

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

End Class
