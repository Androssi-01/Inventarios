import { useEffect, useState } from 'react';
import axios from 'axios';
import ProductoForm from './ProductoForm';

export default function Productos() {
    const [productos, setProductos] = useState(null);
    const [error, setError] = useState(null);
    const [productoEditando, setProductoEditando] = useState(null);

    const cargarProductos = () => {

        axios.get('http://localhost:5205/api/productos')
            .then(response => setProductos(response.data))
            .catch(err => setError('Error al cargar los productos'));
    };

    useEffect(() => {
        cargarProductos();
    }, []);


    const handleEliminar = async (id) => {
        if (window.confirm("¿Estás seguro de eliminar este producto?")) {
            await axios.delete(`http://localhost:5205/api/productos/${id}`);
            cargarProductos();
        }
    };

    if (error) return <div className="alert alert-danger">{error}</div>;

    return (
        <div className="container mt-5">
            <h2 className="mb-4 text-dark fw-bold">Gestión de Productos</h2>

            <ProductoForm
                key={productoEditando ? productoEditando.id : 'nuevo'}
                onProductoCreado={cargarProductos}
                productoEditando={productoEditando}
                onCancelarEdicion={() => setProductoEditando(null)}
            />

            {!productos ? (
                <div className="alert alert-info">Cargando productos...</div>
            ) : (
                <div className="card shadow-sm">
                    <div className="card-body p-0">
                        <table className="table table-hover table-striped mb-0 text-start">
                            <thead className="table-dark">
                                <tr>
                                    <th>Sku</th>
                                    <th>Nombre</th>
                                    <th>Precio</th>
                                    <th>Stock</th>
                                    <th>Descripcion</th>
                                    <th>Estado</th>
                                    <th>Fecha de Creación</th>
                                    <th className="text-center">Acciones</th>
                                </tr>
                            </thead>
                            <tbody className="align-middle">
                                {productos.items.map(prod => (
                                    <tr key={prod.id}>
                                        <td className="fw-bold">{prod.sku}</td>
                                        <td>{prod.nombre}</td>
                                        <td>${parseFloat(prod.precio).toFixed(2)}</td>
                                        <td className={prod.stock < 5 ? 'text-danger fw-bold' : ''}>{prod.stock}</td>
                                        <td>{prod.descripcion}</td>
                                        <td>
                                            <span className={`badge ${prod.activo ? 'bg-success' : 'bg-danger'}`}>
                                                {prod.activo ? 'Activo' : 'Inactivo'}
                                            </span>
                                        </td>
                                        <td>
                                            {new Date(prod.fechaCreacion).toLocaleDateString()}
                                        </td>
                                        <td className="text-center">
                                            <button className="btn btn-sm btn-outline-primary me-2" onClick={() => setProductoEditando(prod)}>
                                                ✏️ Editar
                                            </button>
                                            <button className="btn btn-sm btn-outline-danger" onClick={() => handleEliminar(prod.id)}>
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
