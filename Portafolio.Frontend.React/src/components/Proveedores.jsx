import { useEffect, useState } from "react";
import axios from "axios";
import ProveedorForm from "./ProveedorForm";

export default function Proveedores() {
    const [proveedores, setProveedores] = useState(null);
    const [error, setError] = useState(null);
    const [proveedorEditando, setProveedorEditando] = useState(null);

    const cargarProveedores = () => {

        axios.get('http://localhost:5205/api/proveedores')
            .then(response => setProveedores(response.data))
            .catch(err => setError('Error al cargar los proveedores'));
    };

    useEffect(() => {
        cargarProveedores();
    }, []);


    const handleEliminar = async (id) => {
        if (window.confirm("¿Estás seguro de eliminar este proveedor?")) {
            await axios.delete(`http://localhost:5205/api/proveedores/${id}`);
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
            )}
        </div>
    );
}
