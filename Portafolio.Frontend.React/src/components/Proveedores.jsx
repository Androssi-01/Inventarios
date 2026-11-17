import { useEffect, useState } from "react";
import axiosClient from '../api/axiosClient';
import ProveedorForm from "./ProveedorForm";

export default function Proveedores() {
    const [proveedores, setProveedores] = useState(null);
    const [error, setError] = useState(null);
    const [proveedorEditando, setProveedorEditando] = useState(null);


    const [pagina, setPagina] = useState(1);
    const [totalPaginas, setTotalPaginas] = useState(1);
    const cargarProveedores = () => {

        axiosClient.get(`/proveedores?Pagina=${pagina}&Tamano=5`)
            .then(response => {
                setProveedores(response.data);
                setTotalPaginas(response.data.totalPaginas);
            })
            .catch(err => setError('Error al cargar los proveedores'));
    };

    useEffect(() => {
        cargarProveedores();
    }, [pagina]);


    const handleEliminar = async (id) => {
        if (window.confirm("¿Estás seguro de eliminar este proveedor?")) {
            await axiosClient.delete(`/proveedores/${id}`);
            cargarProveedores();
        }
    };

    if (error) return <div className="alert alert-danger">{error}</div>;

    return (
        <div className="container mt-5">
            <h2 className="mb-4 text-dark fw-bold">Gestión de Proveedores</h2>

            <ProveedorForm
                key={proveedorEditando ? proveedorEditando.id : 'nuevo'}
                onProveedorCreado={cargarProveedores}
                proveedorEditando={proveedorEditando}
                onCancelarEdicion={() => setProveedorEditando(null)}
            />

            {!proveedores ? (
                <div className="alert alert-info">Cargando proveedores...</div>
            ) : (
                <>
                    <div className="card shadow-sm">
                        <div className="card-body p-0">
                            <table className="table table-hover table-striped mb-0 text-start">
                                <thead className="table-dark">
                                    <tr>
                                        <th>Id</th>
                                        <th>Nombre Empresa</th>
                                        <th>Contacto</th>
                                        <th>Email</th>
                                        <th>Teléfono</th>
                                        <th>Estado</th>
                                        <th>Fecha de Alta</th>
                                        <th className="text-center">Acciones</th>
                                    </tr>
                                </thead>
                                <tbody className="align-middle">
                                    {proveedores.items.map(prov => (
                                        <tr key={prov.id}>
                                            <td className="fw-bold">{prov.id}</td>
                                            <td>{prov.nombreEmpresa}</td>
                                            <td>{prov.contacto}</td>
                                            <td>{prov.email}</td>
                                            <td>{prov.telefono}</td>
                                            <td>
                                                <span className={`badge ${prov.activo ? 'bg-success' : 'bg-danger'}`}>
                                                    {prov.activo ? 'Activo' : 'Inactivo'}
                                                </span>
                                            </td>
                                            <td>
                                                {new Date(prov.fechaAlta).toLocaleDateString()}
                                            </td>
                                            <td className="text-center">
                                                <button className="btn btn-sm btn-outline-primary me-2" onClick={() => setProveedorEditando(prov)}>
                                                    ✏️ Editar
                                                </button>
                                                <button className="btn btn-sm btn-outline-danger" onClick={() => handleEliminar(prov.id)}>
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
                            Página <strong>{proveedores.paginaActual}</strong> de <strong>{totalPaginas}</strong>
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
