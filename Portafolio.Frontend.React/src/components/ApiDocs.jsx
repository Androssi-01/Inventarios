import React from 'react';
import SwaggerUI from 'swagger-ui-react';
import 'swagger-ui-react/swagger-ui.css';

const ApiDocs = () => {
    const apiBase = (import.meta.env.VITE_API_URL || '').replace(/\/api\/?$/, '');
    const swaggerUrl = `${apiBase}/openapi/v1.json`;

    return (
        <div className="container mt-5">
            <h2 className="mb-4 text-dark fw-bold">Documentación de la API</h2>
            <div className="card shadow-sm p-4 bg-white">
                <SwaggerUI
                    url={swaggerUrl}
                    requestInterceptor={(request) => {
                        request.url = request.url.replace('http://localhost/', '/');
                        return request;
                    }}
                />
            </div>
        </div>
    );
};

export default ApiDocs;