import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import Clientes from './components/Clientes';
import Productos from './components/Productos';
import Proveedores from './components/Proveedores';
import './App.css';

function App() {
  return (
    <BrowserRouter>
      <nav className="navbar navbar-expand-lg navbar-dark bg-dark mb-4">
        <div className="container">
          <span className="navbar-brand">Portafolio FullStack</span>
          <div className="navbar-nav">
            <Link className="nav-link" to="/">Clientes</Link>
            <Link className="nav-link" to="/productos">Productos</Link>
            <Link className="nav-link" to="/proveedores">Proveedores</Link>
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