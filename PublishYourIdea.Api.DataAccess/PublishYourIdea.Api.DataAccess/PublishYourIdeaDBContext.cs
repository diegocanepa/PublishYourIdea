using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PublishYourIdea.Api.DataAccess.Contracts.Entities;

namespace PublishYourIdea.Api.DataAccess.Contracts
{
    public partial class PublishYourIdeaDBContext : DbContext, IPublishYourIdeaDBContext
    {
        public PublishYourIdeaDBContext()
        {
        }

        public PublishYourIdeaDBContext(DbContextOptions<PublishYourIdeaDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comentario> Comentario { get; set; }
        public virtual DbSet<EmailConfirmationToken> EmailConfirmationToken { get; set; }
        public virtual DbSet<EmailNotificacion> EmailNotificacion { get; set; }
        public virtual DbSet<EstadoIdea> EstadoIdea { get; set; }
        public virtual DbSet<Idea> Idea { get; set; }
        public virtual DbSet<MultimediaComentario> MultimediaComentario { get; set; }
        public virtual DbSet<MultimediaIdea> MultimediaIdea { get; set; }
        public virtual DbSet<Puntuacion> Puntuacion { get; set; }
        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
        public virtual DbSet<Seguidores> Seguidores { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=remotemysql.com;database=XBboynej9U;user=XBboynej9U;password=ZImQgvMqBZ;treattinyasboolean=true;default command timeout=120;sslmode=none", x => x.ServerVersion("8.0.13-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comentario>(entity =>
            {
                entity.HasKey(e => e.IdComentario)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.IdComentario)
                    .HasName("idComentario_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.IdIdea)
                    .HasName("FK1_IDEA_idx");

                entity.HasIndex(e => e.IdUsuario)
                    .HasName("FK2_USUARIO_idx");

                entity.Property(e => e.IdComentario)
                    .HasColumnName("idComentario")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DescripcionComentario)
                    .IsRequired()
                    .HasColumnName("descripcionComentario")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.FechaComentario)
                    .HasColumnName("fechaComentario")
                    .HasColumnType("date");

                entity.Property(e => e.IdIdea)
                    .HasColumnName("idIdea")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("idUsuario")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MeGustas)
                    .HasColumnName("meGustas")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NoMeGustas)
                    .HasColumnName("noMeGustas")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdIdeaNavigation)
                    .WithMany(p => p.Comentario)
                    .HasForeignKey(d => d.IdIdea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1_IDEA");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Comentario)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK2_USUARIO");
            });

            modelBuilder.Entity<EmailConfirmationToken>(entity =>
            {
                entity.HasKey(e => e.Token)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.UserId)
                    .HasName("FK_EMAILTOKEN_USUARIO_idx");

                entity.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("creationDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExpiryDate)
                    .HasColumnName("expiryDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Invalidated)
                    .HasColumnName("invalidated")
                    .HasColumnType("char(1)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.JwtId)
                    .HasColumnName("jwtId")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Used)
                    .HasColumnName("used")
                    .HasColumnType("char(1)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EmailConfirmationToken)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_EMAILTOKEN_USUARIO");
            });

            modelBuilder.Entity<EmailNotificacion>(entity =>
            {
                entity.HasKey(e => e.IdEmailNotificacion)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.UserId)
                    .HasName("FK_EMAILAUDIT_USER_idx");

                entity.Property(e => e.IdEmailNotificacion)
                    .HasColumnName("idEmailNotificacion")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasColumnName("action")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Contenido)
                    .HasColumnName("contenido")
                    .HasColumnType("blob");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EmailNotificacion)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EMAILAUDIT_USER");
            });

            modelBuilder.Entity<EstadoIdea>(entity =>
            {
                entity.HasKey(e => e.IdEstado)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("idEstado_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IdEstado)
                    .HasColumnName("idEstado")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DescripcionEstado)
                    .HasColumnName("descripcionEstado")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.NombreEstado)
                    .IsRequired()
                    .HasColumnName("nombreEstado")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<Idea>(entity =>
            {
                entity.HasKey(e => e.IdIdea)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.IdEstado)
                    .HasName("FK_ESTADOIDEA_idx");

                entity.HasIndex(e => e.IdIdea)
                    .HasName("idIdea_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.IdUsuario)
                    .HasName("idUsuario_idx");

                entity.Property(e => e.IdIdea)
                    .HasColumnName("idIdea")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ComentarioBaja)
                    .HasColumnName("comentarioBaja")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Estrellas)
                    .HasColumnName("estrellas")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FechaBaja)
                    .HasColumnName("fechaBaja")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaPublicacion)
                    .HasColumnName("fechaPublicacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdEstado)
                    .HasColumnName("idEstado")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("idUsuario")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NroIdea)
                    .HasColumnName("nroIdea")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NroVersion)
                    .HasColumnName("nroVersion")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasColumnName("titulo")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.Idea)
                    .HasForeignKey(d => d.IdEstado)
                    .HasConstraintName("FK_ESTADOIDEA");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Idea)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USUARIO");
            });

            modelBuilder.Entity<MultimediaComentario>(entity =>
            {
                entity.HasKey(e => e.IdMultimediaComentario)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.IdMultimediaComentario)
                    .HasName("idMultimediaComentario_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IdMultimediaComentario)
                    .HasColumnName("idMultimediaComentario")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("archivo")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.FechaBaja)
                    .HasColumnName("fechaBaja")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaPublicacion)
                    .HasColumnName("fechaPublicacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdComentario)
                    .HasColumnName("idComentario")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TipoArchivo)
                    .HasColumnName("tipoArchivo")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<MultimediaIdea>(entity =>
            {
                entity.HasKey(e => e.IdMultimediaIdea)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.IdIdea)
                    .HasName("FK_IDEA_idx");

                entity.HasIndex(e => e.IdMultimediaIdea)
                    .HasName("idMultimediaIdea_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IdMultimediaIdea)
                    .HasColumnName("idMultimediaIdea")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Archivo)
                    .HasColumnName("archivo")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Eliminado)
                    .HasColumnName("eliminado")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.FechaPublicacion)
                    .HasColumnName("fechaPublicacion")
                    .HasColumnType("date");

                entity.Property(e => e.IdIdea)
                    .HasColumnName("idIdea")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TipoArchivo)
                    .HasColumnName("tipoArchivo")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.HasOne(d => d.IdIdeaNavigation)
                    .WithMany(p => p.MultimediaIdea)
                    .HasForeignKey(d => d.IdIdea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IDEA");
            });

            modelBuilder.Entity<Puntuacion>(entity =>
            {
                entity.HasKey(e => e.IdPuntuacion)
                    .HasName("PRIMARY");

                entity.HasComment("	");

                entity.HasIndex(e => e.IdIdea)
                    .HasName("FK2_IDEA_idx");

                entity.HasIndex(e => e.IdPuntuacion)
                    .HasName("idPuntuacion_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.IdUsuario)
                    .HasName("FK_USUARIO_idx");

                entity.Property(e => e.IdPuntuacion)
                    .HasColumnName("idPuntuacion")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Estrellas)
                    .HasColumnName("estrellas")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdIdea)
                    .HasColumnName("idIdea")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("idUsuario")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdIdeaNavigation)
                    .WithMany(p => p.Puntuacion)
                    .HasForeignKey(d => d.IdIdea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK2_IDEA");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Puntuacion)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1_USUARIO");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Token)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.UserId)
                    .HasName("FK_USUARIO_idx");

                entity.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("creationDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExpiryDate)
                    .HasColumnName("expiryDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Invalidated)
                    .HasColumnName("invalidated")
                    .HasColumnType("char(1)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.JwtId)
                    .HasColumnName("jwtId")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Used)
                    .HasColumnName("used")
                    .HasColumnType("char(1)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Seguidores>(entity =>
            {
                entity.HasKey(e => e.IdSeguidores)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.IdSeguidores)
                    .HasName("idSeguidores_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.IdUsuarioSeguido)
                    .HasName("FK2_USUARIOSEGUIDO_idx");

                entity.HasIndex(e => e.IdUsuarioSeguidor)
                    .HasName("FK1_USUARIOSEGUIDOR_idx");

                entity.Property(e => e.IdSeguidores)
                    .HasColumnName("idSeguidores")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FechaSeguimiento)
                    .HasColumnName("fechaSeguimiento")
                    .HasColumnType("datetime");

                entity.Property(e => e.FueAceptado)
                    .HasColumnName("fueAceptado")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.IdUsuarioSeguido)
                    .HasColumnName("idUsuarioSeguido")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdUsuarioSeguidor)
                    .HasColumnName("idUsuarioSeguidor")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdUsuarioSeguidoNavigation)
                    .WithMany(p => p.SeguidoresIdUsuarioSeguidoNavigation)
                    .HasForeignKey(d => d.IdUsuarioSeguido)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK2_USUARIOSEGUIDO");

                entity.HasOne(d => d.IdUsuarioSeguidorNavigation)
                    .WithMany(p => p.SeguidoresIdUsuarioSeguidorNavigation)
                    .HasForeignKey(d => d.IdUsuarioSeguidor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1_USUARIOSEGUIDOR");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.IdUsuario)
                    .HasName("idUsuario_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("idUsuario")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Apellido)
                    .IsRequired()
                    .HasColumnName("apellido")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Confirmacion)
                    .HasColumnName("confirmacion")
                    .HasColumnType("char(1)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Contraseña)
                    .IsRequired()
                    .HasColumnName("contraseña")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.FechaBaja)
                    .HasColumnName("fechaBaja")
                    .HasColumnType("date");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnName("fechaCreacion")
                    .HasColumnType("date");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
