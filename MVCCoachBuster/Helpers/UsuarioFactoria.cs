/*
 Clase que contiene la lógica de negocios para la manipulación de los usuarios,
 así como el mapeo de la entidad usuario a UsuarioRegistroDto
 */

using Microsoft.AspNetCore.Identity;
using MVCCoachBuster.Models;
using MVCCoachBuster.ViewModels;

namespace MVCCoachBuster.Helpers
{
    public class UsuarioFactoria
    {
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UsuarioFactoria(IPasswordHasher<Usuario> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public Usuario CrearUsuario(UsuarioRegistroDto usuarioDto)
        {
            var usuario = new Usuario()
            {
                Id = usuarioDto.Id,
                Nombre = usuarioDto.Nombre,
                Correo = usuarioDto.Correo,
                Telefono = usuarioDto.Telefono,
                RolId = usuarioDto.RolId
            };
            usuario.Contrasena = _passwordHasher.HashPassword(usuario, usuarioDto.Contrasena);
            return usuario;
        }

        public UsuarioRegistroDto CrearUsuarioRegistro(Usuario usuario)
        {
             return new UsuarioRegistroDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                RolId = usuario.RolId,
         
            };
        }

        public UsuarioEdicionDto CrearUsuarioEdicion(Usuario usuario)
        {
           return new UsuarioEdicionDto()
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                RolId = usuario.RolId,
             
           };
        }

        public void ActualizarDatosUsuario(UsuarioEdicionDto usuario, Usuario usuarioBd)
        {
            usuarioBd.Nombre = usuario.Nombre;
            usuarioBd.Correo = usuario.Correo;
            usuarioBd.Telefono = usuario.Telefono;
            usuarioBd.RolId = usuario.RolId;
        }

        public CambiarContrasenaViewModel CrearCambiarContrasenaViewModel(Usuario usuario)
        {
            return new CambiarContrasenaViewModel
            {
                Id = usuario.Id,
                Correo = usuario.Correo
            };
        }

        public void ActualizarContrasenaUsuario(CambiarContrasenaViewModel usuarioVM, Usuario usuarioBd)
        {
            usuarioBd.Contrasena = _passwordHasher.HashPassword(usuarioBd, usuarioVM.Contrasena);
        }

    }
}
