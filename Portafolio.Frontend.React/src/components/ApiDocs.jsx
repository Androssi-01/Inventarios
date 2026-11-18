import React from 'react';
import SwaggerUI from 'swagger-ui-react';
import 'swagger-ui-react/swagger-ui.css';

const ApiDocs = () => {
    const swaggerUrl = 'http://localhost:5173/swagger/v1/swagger.json';

    // Esta función se ejecuta cuando Swagger UI termina de cargar
    const onSwaggerLoad = (swaggerUi) => {
        const token = localStorage.getItem('accessToken'); // Asumiendo que guardas el token aquí
        if (token) {
            // 'bearerAuth' es el nombre del esquema de seguridad que definas en tu backend
            swaggerUi.preauthorizeApiKey('bearerAuth', token);
        }
    };

    return (
        <div className="container mt-5">
            <h2 className="mb-4 text-dark fw-bold">Documentación de la API</h2>
            <div className="card shadow-sm p-4">
                <SwaggerUI
                    url={swaggerUrl}
                    onComplete={onSwaggerLoad} // 👈 Se ejecuta al cargar
                />
            </div>
        </div>
    );
};

export default ApiDocs;