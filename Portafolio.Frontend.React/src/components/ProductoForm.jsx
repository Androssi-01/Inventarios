import { useState, useEffect } from 'react';
import axiosClient from '../api/axiosClient';

export default function ProductoForm({ onProductoCreado, productoEditando, onCancelarEdicion }) {
    const [formData, setFormData] = useState({
        sku: '',
        nombre: '',
        descripcion: '',
        precio: 0,
        stock: 0,
        activo: true
    });

    const [error, setError] = useState(null);

    useEffect(() => {
        if (productoEditando) {
            setFormData({
                sku: productoEditando.sku,
                nombre: productoEditando.nombre,
                descripcion: productoEditando.descripcion || '',
                precio: productoEditando.precio,
                stock: productoEditando.stock,
                activo: productoEditando.activo

            });
        } else {
            setFormData({
                sku: '',
                nombre: '',
                descripcion: '',
                precio: 0,
                stock: 0,
                activo: true
            });
        }
    }, [productoEditando]);


    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData({
            ...formData,
            [name]: type === 'checkbox' ? checked : (type === 'number' ? Number(value) : value)
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);

        try {
            if (productoEditando) {

                const updateData = {
                    nombre: formData.nombre,
                    descripcion: formData.descripcion,
                    precio: formData.precio,
                    stock: formData.stock,
                    activo: formData.activo
                };

                await axiosClient.put(`/productos/${productoEditando.id}`, updateData);
            } else {

                await axiosClient.post('/productos', formData);
            }

            setFormData({ sku: '', nombre: '', descripcion: '', precio: 0, stock: 0, activo: true });
            onProductoCreado();
            if (onCancelarEdicion) onCancelarEdicion();

        } catch (err) {
            if (err.response && err.response.data && err.response.data.errors) {
                const mensajes = Object.values(err.response.data.errors).flat().join(', ');
                setError(mensajes);
            } else {
                setError('Error al guardar el producto.');
            }
        }
    };

    return (
        <div className="card shadow-sm mb-5 border-0">
            <div className="card-body p-4 bg-light rounded">
                <h4 className="card-title mb-4 text-success fw-bold">
                    {productoEditando ? '✏️ Editar Producto' : '📦 Crear Producto'}
                </h4>

                {error && <div className="alert alert-danger py-2">{error}</div>}

                <form onSubmit={handleSubmit}>
                    <div className="row">
                        <div className="col-md-6 mb-3">
                            <label className="form-label fw-semibold">SKU</label>
                            <input type="text" name="sku" className="form-control shadow-sm" value={formData.sku} onChange={handleChange} required disabled={productoEditando !== null} />
                        </div>
                        <div className="col-md-6 mb-3">
                            <label className="form-label fw-semibold">Nombre</label>
                            <input type="text" name="nombre" className="form-control shadow-sm" value={formData.nombre} onChange={handleChange} required />
                        </div>
                        <div className="col-md-4 mb-3">
                            <label className="form-label fw-semibold">Precio</label>
                            <input type="number" step="0.01" name="precio" className="form-control shadow-sm" value={formData.precio} onChange={handleChange} required />
                        </div>
                        <div className="col-md-4 mb-3">
                            <label className="form-label fw-semibold">Stock</label>
                            <input type="number" name="stock" className="form-control shadow-sm" value={formData.stock} onChange={handleChange} required />
                        </div>

                        <div className="col-md-4 mb-3">
                            <label className="form-label fw-semibold">Descripción</label>
                            <input type="text" name="descripcion" className="form-control shadow-sm" value={formData.descripcion} onChange={handleChange} />
                        </div>
                    </div>

                    {productoEditando && (
                        <div className="mb-3 form-check">
                            <input type="checkbox" name="activo" className="form-check-input" checked={formData.activo} onChange={handleChange} id="activoCheck" />
                            <label className="form-check-label" htmlFor="activoCheck">Producto Activo</label>
                        </div>
                    )}

                    <div className="mt-2">
                        <button type="submit" className="btn btn-success px-4 me-2">
                            {productoEditando ? 'Actualizar' : 'Guardar'}
                        </button>
                        {productoEditando && (
                            <button type="button" className="btn btn-secondary" onClick={onCancelarEdicion}>
                                Cancelar
                            </button>
                        )}
                    </div>
                </form>
            </div>
        </div>
    );
}