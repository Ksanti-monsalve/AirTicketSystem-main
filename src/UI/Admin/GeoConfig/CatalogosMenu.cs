// src/UI/Admin/GeoConfig/CatalogosMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;

// Gender
using AirTicketSystem.modules.gender.Application.UseCases;
// DocumentType
using AirTicketSystem.modules.documenttype.Application.UseCases;
// AddressType
using AirTicketSystem.modules.addresstype.Application.UseCases;
// PhoneType
using AirTicketSystem.modules.phonetype.Application.UseCases;
// EmailType
using AirTicketSystem.modules.emailtype.Application.UseCases;
// ContactRelationship
using AirTicketSystem.modules.contactrelationship.Application.UseCases;
// ServiceClass
using AirTicketSystem.modules.serviceclass.Application.UseCases;
// WorkerType
using AirTicketSystem.modules.workertype.Application.UseCases;
// Specialty
using AirTicketSystem.modules.specialty.Application.UseCases;
// LuggageType
using AirTicketSystem.modules.luggagetype.Application.UseCases;
// PaymentMethod
using AirTicketSystem.modules.paymentmethod.Application.UseCases;
using AirTicketSystem.modules.gender.Domain.aggregate;
using AirTicketSystem.modules.documenttype.Domain.aggregate;
using AirTicketSystem.modules.addresstype.Domain.aggregate;
using AirTicketSystem.modules.phonetype.Domain.aggregate;
using AirTicketSystem.modules.emailtype.Domain.aggregate;
using AirTicketSystem.modules.luggagetype.Domain.aggregate;
using AirTicketSystem.modules.workertype.Domain.aggregate;
using AirTicketSystem.modules.specialty.Domain.aggregate;
using AirTicketSystem.modules.paymentmethod.Domain.aggregate;
using AirTicketSystem.modules.serviceclass.Domain.aggregate;

namespace AirTicketSystem.UI.Admin.GeoConfig;

public sealed class CatalogosMenu
{
    private readonly IServiceProvider _provider;

    public CatalogosMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Catálogos del Sistema");

            var opcion = SpectreHelper.SeleccionarOpcionTexto(
                "Seleccione un catálogo",
                [
                    "7.1 Géneros",
                    "7.2 Tipos de documento",
                    "7.3 Tipos de teléfono",
                    "7.4 Tipos de email",
                    "7.5 Tipos de dirección",
                    "7.6 Relaciones de contacto",
                    "7.7 Tipos de equipaje",
                    "7.8 Restricciones de equipaje",
                    "7.9 Métodos de pago",
                    "7.10 Tarifas",
                    "Clases de servicio",
                    "Tipos de trabajador",
                    "Especialidades",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "7.1 Géneros":             await GenderMenuAsync();              break;
                case "7.2 Tipos de documento":  await DocumentTypeMenuAsync();        break;
                case "7.3 Tipos de teléfono":   await PhoneTypeMenuAsync();           break;
                case "7.4 Tipos de email":      await EmailTypeMenuAsync();           break;
                case "7.5 Tipos de dirección":  await AddressTypeMenuAsync();         break;
                case "7.6 Relaciones de contacto": await ContactRelationshipMenuAsync(); break;
                case "7.7 Tipos de equipaje":   await LuggageTypeMenuAsync();         break;
                case "7.8 Restricciones de equipaje": SpectreHelper.MostrarInfo("Ver sección 2 → Aerolíneas y Flota → Restricciones."); SpectreHelper.EsperarTecla(); break;
                case "7.9 Métodos de pago":     await PaymentMethodMenuAsync();       break;
                case "7.10 Tarifas":            SpectreHelper.MostrarInfo("Ver sección 2 → Aerolíneas y Flota → Tarifas."); SpectreHelper.EsperarTecla(); break;
                case "Clases de servicio":      await ServiceClassMenuAsync();        break;
                case "Tipos de trabajador":     await WorkerTypeMenuAsync();          break;
                case "Especialidades":          await SpecialtyMenuAsync();           break;
                case "Volver":                  return;
            }
        }
    }

    // ── Géneros ───────────────────────────────────────────────────────────────

    private async Task GenderMenuAsync()
    {
        await CrudSimpleAsync(
            titulo: "Géneros",
            listar: async scope =>
            {
                var lista = await scope.GetRequiredService<GetAllGendersUseCase>().ExecuteAsync();
                var t = SpectreHelper.CrearTabla("ID", "Nombre");
                foreach (var x in lista) SpectreHelper.AgregarFila(t, x.Id.ToString(), x.Nombre.Valor);
                return (lista.Count, t);
            },
            crear: async scope =>
            {
                var nombre = SpectreHelper.PedirTexto("Nombre del género");
                var r = await scope.GetRequiredService<CreateGenderUseCase>().ExecuteAsync(nombre);
                return r.Nombre.Valor;
            },
            editar: async scope =>
            {
                var genero = await SelectorUI.SeleccionarGeneroAsync(scope);
                if (genero is null) {
                    SpectreHelper.MostrarInfo("Operación cancelada.");
                    throw new InvalidOperationException("Género no seleccionado.");
                }
                var id = genero.Id;
                var nombre = SpectreHelper.PedirTexto("Nuevo nombre");
                var r = await scope.GetRequiredService<UpdateGenderUseCase>().ExecuteAsync(id, nombre);
                return r.Nombre.Valor;
            },
            eliminar: async scope =>
            {
                var genero = await SelectorUI.SeleccionarGeneroAsync(scope);
                if (genero is null) {
                    SpectreHelper.MostrarInfo("Operación cancelada.");
                    throw new InvalidOperationException("Género no seleccionado.");
                }
                var id = genero.Id;
                await scope.GetRequiredService<DeleteGenderUseCase>().ExecuteAsync(id);
            });
    }

    // ── Tipos de documento ────────────────────────────────────────────────────

    private async Task DocumentTypeMenuAsync()
    {
        await CrudSimpleAsync(
            titulo: "Tipos de Documento",
            listar: async scope =>
            {
                var lista = await scope.GetRequiredService<GetAllDocumentTypesUseCase>().ExecuteAsync();
                var t = SpectreHelper.CrearTabla("ID", "Descripción");
                foreach (var x in lista) SpectreHelper.AgregarFila(t, x.Id.ToString(), x.Descripcion.Valor);
                return (lista.Count, t);
            },
            crear: async scope =>
            {
                var desc = SpectreHelper.PedirTexto("Descripción");
                var r = await scope.GetRequiredService<CreateDocumentTypeUseCase>().ExecuteAsync(desc);
                return r.Descripcion.Valor;
            },
            editar: async scope =>
            {
                var documentType = await SelectorUI.SeleccionarTipoDocumentoAsync(scope);
                if (documentType is null) {
                    SpectreHelper.MostrarInfo("Operación cancelada.");
                    throw new InvalidOperationException("Tipo de documento no seleccionado.");
                }
                var id = documentType.Id;
                var desc = SpectreHelper.PedirTexto("Nueva descripción");
                var r = await scope.GetRequiredService<UpdateDocumentTypeUseCase>().ExecuteAsync(id, desc);
                return r.Descripcion.Valor;
            },
            eliminar: async scope =>
            {
                var documentType = await SelectorUI.SeleccionarTipoDocumentoAsync(scope);
                if (documentType is null) {
                    SpectreHelper.MostrarInfo("Operación cancelada.");
                    throw new InvalidOperationException("Tipo de documento no seleccionado.");
                }
                var id = documentType.Id;
                await scope.GetRequiredService<DeleteDocumentTypeUseCase>().ExecuteAsync(id);
            });
    }

    // ── Tipos de dirección ────────────────────────────────────────────────────

    private async Task AddressTypeMenuAsync()
    {
        await CrudSimpleAsync(
            titulo: "Tipos de Dirección",
            listar: async scope =>
            {
                var lista = await scope.GetRequiredService<GetAllAddressTypesUseCase>().ExecuteAsync();
                var t = SpectreHelper.CrearTabla("ID", "Descripción");
                foreach (var x in lista) SpectreHelper.AgregarFila(t, x.Id.ToString(), x.Descripcion.Valor);
                return (lista.Count, t);
            },
            crear: async scope =>
            {
                var desc = SpectreHelper.PedirTexto("Descripción");
                var r = await scope.GetRequiredService<CreateAddressTypeUseCase>().ExecuteAsync(desc);
                return r.Descripcion.Valor;
            },
            editar: async scope =>
            {
                var addressType = await SelectorUI.SeleccionarTipoDireccionAsync(scope);
                if (addressType is null) {
                    SpectreHelper.MostrarInfo("Operación cancelada.");
                    throw new InvalidOperationException("Tipo de dirección no seleccionado.");
                }
                var id = addressType.Id;
                var desc = SpectreHelper.PedirTexto("Nueva descripción");
                var r = await scope.GetRequiredService<UpdateAddressTypeUseCase>().ExecuteAsync(id, desc);
                return r.Descripcion.Valor;
            },
            eliminar: async scope =>
            {
                var addressType = await SelectorUI.SeleccionarTipoDireccionAsync(scope);
                if (addressType is null) {
                    SpectreHelper.MostrarInfo("Operación cancelada.");
                    throw new InvalidOperationException("Tipo de dirección no seleccionado.");
                }
                var id = addressType.Id;
                await scope.GetRequiredService<DeleteAddressTypeUseCase>().ExecuteAsync(id);
            });
    }

    // ── Tipos de teléfono ─────────────────────────────────────────────────────

    private async Task PhoneTypeMenuAsync()
    {
        await CrudSimpleAsync(
            titulo: "Tipos de Teléfono",
            listar: async scope =>
            {
                var lista = await scope.GetRequiredService<GetAllPhoneTypesUseCase>().ExecuteAsync();
                var t = SpectreHelper.CrearTabla("ID", "Descripción");
                foreach (var x in lista) SpectreHelper.AgregarFila(t, x.Id.ToString(), x.Descripcion.Valor);
                return (lista.Count, t);
            },
            crear: async scope =>
            {
                var desc = SpectreHelper.PedirTexto("Descripción");
                var r = await scope.GetRequiredService<CreatePhoneTypeUseCase>().ExecuteAsync(desc);
                return r.Descripcion.Valor;
            },
            editar: async scope =>
            {
                var phoneType = await SelectorUI.SeleccionarTipoTelefonoAsync(scope);
                if (phoneType is null) {
                    SpectreHelper.MostrarInfo("Operación cancelada.");
                    throw new InvalidOperationException("Tipo de teléfono no seleccionado.");
                }
                var id = phoneType.Id;
                var desc = SpectreHelper.PedirTexto("Nueva descripción");
                var r = await scope.GetRequiredService<UpdatePhoneTypeUseCase>().ExecuteAsync(id, desc);
                return r.Descripcion.Valor;
            },
            eliminar: async scope =>
            {
                var phoneType = await SelectorUI.SeleccionarTipoTelefonoAsync(scope);
                if (phoneType is null) {
                    SpectreHelper.MostrarInfo("Operación cancelada.");
                    throw new InvalidOperationException("Tipo de teléfono no seleccionado.");
                }
                var id = phoneType.Id;
                await scope.GetRequiredService<DeletePhoneTypeUseCase>().ExecuteAsync(id);
            });
    }

    // ── Tipos de email ────────────────────────────────────────────────────────

    private async Task EmailTypeMenuAsync()
    {
        await CrudSimpleAsync(
            titulo: "Tipos de Email",
            listar: async scope =>
            {
                var lista = await scope.GetRequiredService<GetAllEmailTypesUseCase>().ExecuteAsync();
                var t = SpectreHelper.CrearTabla("ID", "Descripción");
                foreach (var x in lista) SpectreHelper.AgregarFila(t, x.Id.ToString(), x.Descripcion.Valor);
                return (lista.Count, t);
            },
            crear: async scope =>
            {
                var desc = SpectreHelper.PedirTexto("Descripción");
                var r = await scope.GetRequiredService<CreateEmailTypeUseCase>().ExecuteAsync(desc);
                return r.Descripcion.Valor;
            },
            editar: async scope =>
            {
                var emailType = await SelectorUI.SeleccionarTipoEmailAsync(scope);
                if (emailType is null) {
                    SpectreHelper.MostrarInfo("Operación cancelada.");
                    throw new InvalidOperationException("Tipo de email no seleccionado.");
                }
                var id = emailType.Id;
                var desc = SpectreHelper.PedirTexto("Nueva descripción");
                var r = await scope.GetRequiredService<UpdateEmailTypeUseCase>().ExecuteAsync(id, desc);
                return r.Descripcion.Valor;
            },
            eliminar: async scope =>
            {
                var emailType = await SelectorUI.SeleccionarTipoEmailAsync(scope);
                if (emailType is null) {
                    SpectreHelper.MostrarInfo("Operación cancelada.");
                    throw new InvalidOperationException("Tipo de email no seleccionado.");
                }
                var id = emailType.Id;
                await scope.GetRequiredService<DeleteEmailTypeUseCase>().ExecuteAsync(id);
            });
    }

    // ── Relaciones de contacto ────────────────────────────────────────────────

    private async Task ContactRelationshipMenuAsync()
    {
        await CrudSimpleAsync(
            titulo: "Relaciones de Contacto",
            listar: async scope =>
            {
                var lista = await scope.GetRequiredService<GetAllContactRelationshipsUseCase>().ExecuteAsync();
                var t = SpectreHelper.CrearTabla("ID", "Descripción");
                foreach (var x in lista) SpectreHelper.AgregarFila(t, x.Id.ToString(), x.Descripcion.Valor);
                return (lista.Count, t);
            },
            crear: async scope =>
            {
                var desc = SpectreHelper.PedirTexto("Descripción");
                var r = await scope.GetRequiredService<CreateContactRelationshipUseCase>().ExecuteAsync(desc);
                return r.Descripcion.Valor;
            },
            editar: async scope =>
            {
                var relationship = await SelectorUI.SeleccionarRelacionContactoAsync(scope);
                if (relationship is null) {
                    SpectreHelper.MostrarInfo("Operación cancelada.");
                    throw new InvalidOperationException("Relación de contacto no seleccionada.");
                }
                var id = relationship.Id;
                var desc = SpectreHelper.PedirTexto("Nueva descripción");
                var r = await scope.GetRequiredService<UpdateContactRelationshipUseCase>().ExecuteAsync(id, desc);
                return r.Descripcion.Valor;
            },
            eliminar: async scope =>
            {
                var relationship = await SelectorUI.SeleccionarRelacionContactoAsync(scope);
                if (relationship is null) {
                    SpectreHelper.MostrarInfo("Operación cancelada.");
                    throw new InvalidOperationException("Relación de contacto no seleccionada.");
                }
                var id = relationship.Id;
                await scope.GetRequiredService<DeleteContactRelationshipUseCase>().ExecuteAsync(id);
            });
    }

    // ── Clases de servicio ────────────────────────────────────────────────────

    private async Task ServiceClassMenuAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Clases de Servicio");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Acción",
                ["Listar", "Crear", "Editar", "Eliminar", "Volver"]);

            if (opcion == "Volver") return;

            switch (opcion)
            {
                case "Listar":
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        await using var sc = _provider.CreateAsyncScope();
                        var lista = await sc.ServiceProvider.GetRequiredService<GetAllServiceClassesUseCase>().ExecuteAsync();
                        if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin registros."); SpectreHelper.EsperarTecla(); return; }
                        var t = SpectreHelper.CrearTabla("ID", "Nombre", "Código", "Descripción");
                        foreach (var x in lista)
                            SpectreHelper.AgregarFila(t, x.Id.ToString(), x.Nombre.Valor, x.Codigo.Valor, x.Descripcion?.Valor ?? "-");
                        SpectreHelper.MostrarTabla(t);
                        SpectreHelper.EsperarTecla();
                    });
                    break;

                case "Crear":
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        var nombre = SpectreHelper.PedirTexto("Nombre (ej: Ejecutiva)");
                        var codigo = SpectreHelper.PedirTexto("Código (ej: J)");
                        var desc   = SpectreHelper.PedirTexto("Descripción (opcional, Enter para omitir)");
                        string? descOpc = string.IsNullOrWhiteSpace(desc) ? null : desc;

                        await using var sc = _provider.CreateAsyncScope();
                        var r = await sc.ServiceProvider.GetRequiredService<CreateServiceClassUseCase>()
                            .ExecuteAsync(nombre, codigo, descOpc);
                        SpectreHelper.MostrarExito($"Clase '{r.Nombre.Valor}' creada (ID {r.Id}).");
                    });
                    SpectreHelper.EsperarTecla();
                    break;

                case "Editar":
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        var id     = SpectreHelper.PedirEntero("ID de la clase");
                        var nombre = SpectreHelper.PedirTexto("Nuevo nombre");
                        var desc   = SpectreHelper.PedirTexto("Nueva descripción (opcional, Enter para omitir)");
                        string? descOpc = string.IsNullOrWhiteSpace(desc) ? null : desc;

                        await using var sc = _provider.CreateAsyncScope();
                        var r = await sc.ServiceProvider.GetRequiredService<UpdateServiceClassUseCase>()
                            .ExecuteAsync(id, nombre, descOpc);
                        SpectreHelper.MostrarExito($"Clase '{r.Nombre.Valor}' actualizada.");
                    });
                    SpectreHelper.EsperarTecla();
                    break;

                case "Eliminar":
                    if (!SpectreHelper.Confirmar("¿Confirma la eliminación?")) { SpectreHelper.EsperarTecla(); break; }
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        var id = SpectreHelper.PedirEntero("ID a eliminar");
                        await using var sc = _provider.CreateAsyncScope();
                        await sc.ServiceProvider.GetRequiredService<DeleteServiceClassUseCase>().ExecuteAsync(id);
                        SpectreHelper.MostrarExito("Clase eliminada.");
                    });
                    SpectreHelper.EsperarTecla();
                    break;
            }
        }
    }

    // ── Tipos de trabajador ───────────────────────────────────────────────────

    private async Task WorkerTypeMenuAsync()
    {
        await CrudSimpleAsync(
            titulo: "Tipos de Trabajador",
            listar: async scope =>
            {
                var lista = await scope.GetRequiredService<GetAllWorkerTypesUseCase>().ExecuteAsync();
                var t = SpectreHelper.CrearTabla("ID", "Nombre");
                foreach (var x in lista) SpectreHelper.AgregarFila(t, x.Id.ToString(), x.Nombre.Valor);
                return (lista.Count, t);
            },
            crear: async scope =>
            {
                var nombre = SpectreHelper.PedirTexto("Nombre");
                var r = await scope.GetRequiredService<CreateWorkerTypeUseCase>().ExecuteAsync(nombre);
                return r.Nombre.Valor;
            },
            editar: async scope =>
            {
                var workerType = await SelectorUI.SeleccionarTipoTrabajadorAsync(scope);
                if (workerType is null) {
                    SpectreHelper.MostrarInfo("Operación cancelada.");
                    throw new InvalidOperationException("Tipo de trabajador no seleccionado.");
                }
                var id = workerType.Id;
                var nombre = SpectreHelper.PedirTexto("Nuevo nombre");
                var r = await scope.GetRequiredService<UpdateWorkerTypeUseCase>().ExecuteAsync(id, nombre);
                return r.Nombre.Valor;
            },
            eliminar: async scope =>
            {
                var workerType = await SelectorUI.SeleccionarTipoTrabajadorAsync(scope);
                if (workerType is null) {
                    SpectreHelper.MostrarInfo("Operación cancelada.");
                    throw new InvalidOperationException("Tipo de trabajador no seleccionado.");
                }
                var id = workerType.Id;
                await scope.GetRequiredService<DeleteWorkerTypeUseCase>().ExecuteAsync(id);
            });
    }

    // ── Especialidades ────────────────────────────────────────────────────────

    private async Task SpecialtyMenuAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Especialidades");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Acción",
                ["Listar", "Crear", "Editar", "Eliminar", "Volver"]);

            if (opcion == "Volver") return;

            switch (opcion)
            {
                case "Listar":
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        await using var sc = _provider.CreateAsyncScope();
                        var lista = await sc.ServiceProvider.GetRequiredService<GetAllSpecialtiesUseCase>().ExecuteAsync();
                        if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin registros."); SpectreHelper.EsperarTecla(); return; }
                        var t = SpectreHelper.CrearTabla("ID", "Nombre", "TipoTrabajadorID");
                        foreach (var x in lista)
                            SpectreHelper.AgregarFila(t, x.Id.ToString(), x.Nombre.Valor,
                                x.TipoTrabajadorId?.ToString() ?? "General");
                        SpectreHelper.MostrarTabla(t);
                        SpectreHelper.EsperarTecla();
                    });
                    break;

                case "Crear":
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        var nombre = SpectreHelper.PedirTexto("Nombre de la especialidad");
                        var tipoStr = SpectreHelper.PedirTexto("ID tipo trabajador (opcional, Enter para omitir)");
                        int? tipoId = int.TryParse(tipoStr, out var t) && t > 0 ? t : null;

                        await using var sc = _provider.CreateAsyncScope();
                        var r = await sc.ServiceProvider.GetRequiredService<CreateSpecialtyUseCase>()
                            .ExecuteAsync(nombre, tipoId);
                        SpectreHelper.MostrarExito($"Especialidad '{r.Nombre.Valor}' creada (ID {r.Id}).");
                    });
                    SpectreHelper.EsperarTecla();
                    break;

                case "Editar":
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        var id     = SpectreHelper.PedirEntero("ID de la especialidad");
                        var nombre = SpectreHelper.PedirTexto("Nuevo nombre");
                        var tipoStr = SpectreHelper.PedirTexto("ID tipo trabajador (opcional, Enter para omitir)");
                        int? tipoId = int.TryParse(tipoStr, out var t) && t > 0 ? t : null;

                        await using var sc = _provider.CreateAsyncScope();
                        var r = await sc.ServiceProvider.GetRequiredService<UpdateSpecialtyUseCase>()
                            .ExecuteAsync(id, nombre, tipoId);
                        SpectreHelper.MostrarExito($"Especialidad '{r.Nombre.Valor}' actualizada.");
                    });
                    SpectreHelper.EsperarTecla();
                    break;

                case "Eliminar":
                    if (!SpectreHelper.Confirmar("¿Confirma la eliminación?")) { SpectreHelper.EsperarTecla(); break; }
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        var id = SpectreHelper.PedirEntero("ID a eliminar");
                        await using var sc = _provider.CreateAsyncScope();
                        await sc.ServiceProvider.GetRequiredService<DeleteSpecialtyUseCase>().ExecuteAsync(id);
                        SpectreHelper.MostrarExito("Especialidad eliminada.");
                    });
                    SpectreHelper.EsperarTecla();
                    break;
            }
        }
    }

    // ── Tipos de equipaje ─────────────────────────────────────────────────────

    private async Task LuggageTypeMenuAsync()
    {
        await CrudSimpleAsync(
            titulo: "Tipos de Equipaje",
            listar: async scope =>
            {
                var lista = await scope.GetRequiredService<GetAllLuggageTypesUseCase>().ExecuteAsync();
                var t = SpectreHelper.CrearTabla("ID", "Nombre");
                foreach (var x in lista) SpectreHelper.AgregarFila(t, x.Id.ToString(), x.Nombre.Valor);
                return (lista.Count, t);
            },
            crear: async scope =>
            {
                var nombre = SpectreHelper.PedirTexto("Nombre");
                var r = await scope.GetRequiredService<CreateLuggageTypeUseCase>().ExecuteAsync(nombre);
                return r.Nombre.Valor;
            },
            editar: async scope =>
            {
                var luggageType = await SelectorUI.SeleccionarTipoEquipajeAsync(scope);
                if (luggageType is null) {
                    SpectreHelper.MostrarInfo("Operación cancelada.");
                    throw new InvalidOperationException("Tipo de equipaje no seleccionado.");
                }
                var id = luggageType.Id;
                var nombre = SpectreHelper.PedirTexto("Nuevo nombre");
                var r = await scope.GetRequiredService<UpdateLuggageTypeUseCase>().ExecuteAsync(id, nombre);
                return r.Nombre.Valor;
            },
            eliminar: async scope =>
            {
                var luggageType = await SelectorUI.SeleccionarTipoEquipajeAsync(scope);
                if (luggageType is null) {
                    SpectreHelper.MostrarInfo("Operación cancelada.");
                    throw new InvalidOperationException("Tipo de equipaje no seleccionado.");
                }
                var id = luggageType.Id;
                await scope.GetRequiredService<DeleteLuggageTypeUseCase>().ExecuteAsync(id);
            });
    }

    // ── Métodos de pago ───────────────────────────────────────────────────────

    private async Task PaymentMethodMenuAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Métodos de Pago");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Acción",
                ["Buscar por ID", "Crear", "Editar", "Volver"]);

            if (opcion == "Volver") return;

            switch (opcion)
            {
                case "Buscar por ID":
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        var id = SpectreHelper.PedirEntero("ID del método de pago");
                        await using var sc = _provider.CreateAsyncScope();
                        var m = await sc.ServiceProvider.GetRequiredService<GetPaymentMethodByIdUseCase>().ExecuteAsync(id);
                        var t = SpectreHelper.CrearTabla("ID", "Nombre");
                        SpectreHelper.AgregarFila(t, m.Id.ToString(), m.Nombre.Valor);
                        SpectreHelper.MostrarTabla(t);
                        SpectreHelper.EsperarTecla();
                    });
                    break;

                case "Crear":
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        var nombre = SpectreHelper.PedirTexto("Nombre del método de pago");
                        await using var sc = _provider.CreateAsyncScope();
                        var m = await sc.ServiceProvider.GetRequiredService<CreatePaymentMethodUseCase>().ExecuteAsync(nombre);
                        SpectreHelper.MostrarExito($"Método '{m.Nombre.Valor}' creado (ID {m.Id}).");
                    });
                    SpectreHelper.EsperarTecla();
                    break;

                case "Editar":
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        var id     = SpectreHelper.PedirEntero("ID del método de pago");
                        var nombre = SpectreHelper.PedirTexto("Nuevo nombre");
                        await using var sc = _provider.CreateAsyncScope();
                        var m = await sc.ServiceProvider.GetRequiredService<UpdatePaymentMethodUseCase>().ExecuteAsync(id, nombre);
                        SpectreHelper.MostrarExito($"Método '{m.Nombre.Valor}' actualizado.");
                    });
                    SpectreHelper.EsperarTecla();
                    break;
            }
        }
    }

    // ── Helper genérico CRUD simple ───────────────────────────────────────────

    private async Task CrudSimpleAsync(
        string titulo,
        Func<IServiceProvider, Task<(int count, Spectre.Console.Table tabla)>> listar,
        Func<IServiceProvider, Task<string>> crear,
        Func<IServiceProvider, Task<string>> editar,
        Func<IServiceProvider, Task> eliminar)
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo(titulo);

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Acción",
                ["Listar", "Crear", "Editar", "Eliminar", "Volver"]);

            if (opcion == "Volver") return;

            switch (opcion)
            {
                case "Listar":
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        await using var sc = _provider.CreateAsyncScope();
                        var (count, tabla) = await listar(sc.ServiceProvider);
                        if (count == 0) { SpectreHelper.MostrarInfo("Sin registros."); SpectreHelper.EsperarTecla(); return; }
                        SpectreHelper.MostrarTabla(tabla);
                        SpectreHelper.EsperarTecla();
                    });
                    break;

                case "Crear":
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        await using var sc = _provider.CreateAsyncScope();
                        var nombre = await crear(sc.ServiceProvider);
                        SpectreHelper.MostrarExito($"'{nombre}' creado correctamente.");
                    });
                    SpectreHelper.EsperarTecla();
                    break;

                case "Editar":
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        await using var sc = _provider.CreateAsyncScope();
                        var nombre = await editar(sc.ServiceProvider);
                        SpectreHelper.MostrarExito($"'{nombre}' actualizado correctamente.");
                    });
                    SpectreHelper.EsperarTecla();
                    break;

                case "Eliminar":
                    if (!SpectreHelper.Confirmar("¿Confirma la eliminación?")) { SpectreHelper.EsperarTecla(); break; }
                    await ConsoleErrorHandler.ExecuteAsync(async () =>
                    {
                        await using var sc = _provider.CreateAsyncScope();
                        await eliminar(sc.ServiceProvider);
                        SpectreHelper.MostrarExito("Registro eliminado correctamente.");
                    });
                    SpectreHelper.EsperarTecla();
                    break;
            }
        }
    }
}
