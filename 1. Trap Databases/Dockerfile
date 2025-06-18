# Use Alpine Linux as base image
FROM alpine:latest

# Set environment variables
ENV POSTGRES_USER=plntrapuser
ENV POSTGRES_PASSWORD=plntrappassword

# Install PostgreSQL
RUN apk update && apk add postgresql16 postgresql16-contrib

# Initialize database cluster and start PostgreSQL
RUN mkdir /run/postgresql && chown postgres /run/postgresql
USER postgres
RUN initdb -D /var/lib/postgresql/data --locale=en_US.utf8 && \
    sed -i "s/#listen_addresses = 'localhost'/listen_addresses = '*'/" /var/lib/postgresql/data/postgresql.conf && \
    echo "host    all             all             0.0.0.0/0               md5" >> /var/lib/postgresql/data/pg_hba.conf && \
    pg_ctl -D /var/lib/postgresql/data -l /var/lib/postgresql/logfile start && \
    psql -U postgres -p 5432 -c "CREATE USER ${POSTGRES_USER} WITH ENCRYPTED PASSWORD '${POSTGRES_PASSWORD}';"

# Expose PostgreSQL port
EXPOSE 5432

# Start PostgreSQL
CMD ["postgres", "-D", "/var/lib/postgresql/data", "-c", "config_file=/var/lib/postgresql/data/postgresql.conf"]
