# 1. Etapa de compilación (Node)
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# 2. Etapa de ejecución (Nginx)
FROM nginx:alpine
# Borra la configuración por defecto de Nginx
RUN rm /etc/nginx/conf.d/default.conf
# Copia nuestra nueva configuración
COPY nginx.conf /etc/nginx/conf.d/default.conf
# Copia los archivos compilados de React
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]