import { useEffect, useState } from "react";
import axiosClient from '../api/axiosClient';
import ClienteForm from "./ClienteForm";

export default function Clientes() {
    const [clientes, setClientes] = useState(null);
    const [error, setError] = useState(null);
    const [clienteEditando, setClienteEditando] = useState(null);

    const [pagina, setPagina] = useState(1);
    const [totalPaginas, setTotalPaginas] = useState(1);



    const cargarClientes = () => {

        axiosClient.get(`/clientes?Pagina=${pagina}&Tamano=5`)
            .then(response => {
                setClientes(response.data);
                setTotalPaginas(response.data.totalPaginas);
            })
            .catch(err => setError('Error al cargar los clientes'));
    };

    useEffect(() => {
        cargarClientes();
    }, [pagina]);


    const handleEliminar = async (id) => {
        if (window.confirm("¿Estás seguro de eliminar este cliente?")) {
            await axiosClient.delete(`/clientes/${id}`);
            cargarClientes();
        }
    };

    if (error) return <div className="alert alert-danger">{error}</div>;

    return (
        <div className="container mt-5">
            <h2 className="mb-4 text-dark fw-bold">Gestión de Clientes</h2>

            <ClienteForm
                key={clienteEditando ? clienteEditando.id : 'nuevo'}
                onClienteCreado={cargarClientes}
                clienteEditando={clienteEditando}
                onCancelarEdicion={() => setClienteEditando(null)}
            />

            {!clientes ? (
                <div className="alert alert-info">Cargando clientes...</div>
            ) : (
                <>
                    <div className="card shadow-sm mb-3">
                        <div className="card-body p-0">
                            <table className="table table-hover table-striped mb-0 text-start">
                                <thead className="table-dark">
                                    <tr>
                                        <th>ID</th>
                                        <th>Nombre</th>
                                        <th>Email</th>
                                        <th>Teléfono</th>
                                        <th>Estado</th>
                                        <th className="text-center">Acciones</th>
                                    </tr>
                                </thead>
                                <tbody className="align-middle">
                                    {clientes.items.map(cliente => (
                                        <tr key={cliente.id}>
                                            <td className="fw-bold">{cliente.id}</td>
                                            <td className="fw-semibold">{cliente.nombre}</td>
                                            <td>{cliente.email}</td>
                                            <td>{cliente.telefono || 'N/A'}</td>
                                            <td>
                                                <span className={`badge ${cliente.activo ? 'bg-success' : 'bg-danger'}`}>
                                                    {cliente.activo ? 'Activo' : 'Inactivo'}
                                                </span>
                                            </td>
                                            <td className="text-center">
                                                <button className="btn btn-sm btn-outline-primary me-2" onClick={() => setClienteEditando(cliente)}>
                                                    ✏️ Editar
                                                </button>
                                                <button className="btn btn-sm btn-outline-danger" onClick={() => handleEliminar(cliente.id)}>
                                                    🗑️ Eliminar
                                                </button>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <div className="d-flex justify-content-between align-items-center">
                        <span className="text-muted">
                            Página <strong>{clientes.paginaActual}</strong> de <strong>{totalPaginas}</strong>
                        </span>
                        <div>
                            <button
                                className="btn btn-outline-secondary me-2"
                                disabled={pagina === 1}
                                onClick={() => setPagina(pagina - 1)}
                            >
                                ⬅️ Anterior
                            </button>
                            <button
                                className="btn btn-outline-secondary"
                                disabled={pagina === totalPaginas}
                                onClick={() => setPagina(pagina + 1)}
                            >
                                Siguiente ➡️
                            </button>
                        </div>
                    </div>
                </>
            )}
        </div>
    );
}