import { useEffect, useState } from 'react';
import axiosClient from '../api/axiosClient';
import ProductoForm from './ProductoForm';

export default function Productos() {
    const [productos, setProductos] = useState(null);
    const [error, setError] = useState(null);
    const [productoEditando, setProductoEditando] = useState(null);

    const [pagina, setPagina] = useState(1);
    const [totalPaginas, setTotalPaginas] = useState(1);

    const cargarProductos = () => {

        axiosClient.get(`/productos?Pagina=${pagina}&Tamano=5`)
            .then(response => {
                setProductos(response.data);
                setTotalPaginas(response.data.totalPaginas);
            })
            .catch(err => setError('Error al cargar los productos'));
    };

    useEffect(() => {
        cargarProductos();
    }, [pagina]);


    const handleEliminar = async (id) => {
        if (window.confirm("¿Estás seguro de eliminar este producto?")) {
            await axiosClient.delete(`/productos/${id}`);
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
                <>
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

                    <div className="d-flex justify-content-between align-items-center">
                        <span className="text-muted">
                            Página <strong>{productos.paginaActual}</strong> de <strong>{totalPaginas}</strong>
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
