FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY ./app/image/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf
