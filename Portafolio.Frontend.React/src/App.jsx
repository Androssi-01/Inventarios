import { BrowserRouter, Routes, Route, Link, useNavigate } from 'react-router-dom';
import Clientes from './components/Clientes';
import Productos from './components/Productos';
import Proveedores from './components/Proveedores';
import Login from './components/Login';
import './App.css';

function App() {
  const token = localStorage.getItem('accessToken');

  const handleLogout = () => {
    localStorage.removeItem('accessToken');
    window.location.reload();
  };
  if (!token) {
    return <Login />;
  }
  return (
    <BrowserRouter>
      <nav className="navbar navbar-expand-lg navbar-dark bg-dark mb-4">
        <div className="container">
          <span className="navbar-brand">Portafolio FullStack</span>
          <div className="navbar-nav w-100 d-flex align-items-center">
            <Link className="nav-link" to="/">Clientes</Link>
            <Link className="nav-link" to="/productos">Productos</Link>
            <Link className="nav-link" to="/proveedores">Proveedores</Link>

            <button onClick={handleLogout} className="btn btn-sm btn-outline-danger ms-auto">
              Cerrar Sesión
            </button>
          </div>
        </div>
      </nav>

      <Routes>
        <Route path="/" element={<Clientes />} />
        <Route path="/productos" element={<Productos />} />
        <Route path="/proveedores" element={<Proveedores />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;