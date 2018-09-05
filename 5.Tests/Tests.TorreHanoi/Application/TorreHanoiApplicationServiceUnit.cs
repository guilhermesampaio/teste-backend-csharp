using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using Application.TorreHanoi.Implementation;
using Application.TorreHanoi.Interface;
using Domain.TorreHanoi.Interface.Service;
using Infrastructure.TorreHanoi.ImagemHelper;
using Infrastructure.TorreHanoi.Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.TorreHanoi.Application
{
    [TestClass]
    public class TorreHanoiApplicationServiceUnit
    {
        private const string CategoriaTeste = "Application/Service/TorreHanoi";

        private ITorreHanoiApplicationService _service;
        private Mock<IDesignerService> _mockDesignerService;
        private Mock<ITorreHanoiDomainService> _mockTorreHanoiDomainService;
        private Mock<ILogger> _mockLogger;

        [TestInitialize]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger>();
            _mockDesignerService = new Mock<IDesignerService>();
            _mockTorreHanoiDomainService = new Mock<ITorreHanoiDomainService>();

            _mockLogger.Setup(s => s.Logar(It.IsAny<string>(), It.IsAny<TipoLog>()));
            _mockTorreHanoiDomainService.Setup(s => s.Criar(It.IsAny<int>())).Returns(Guid.NewGuid);
            _mockTorreHanoiDomainService.Setup(s => s.ObterPor(It.IsAny<Guid>())).Returns(() => CriarTorreHanoi());
            _mockTorreHanoiDomainService.Setup(s => s.ObterTodos()).Returns(() => new List<global::Domain.TorreHanoi.TorreHanoi> { CriarTorreHanoi() });

            _service = new TorreHanoiApplicationService(_mockTorreHanoiDomainService.Object, _mockLogger.Object, _mockDesignerService.Object);
        }



        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void AdicionarNovoProcesso_Deve_Retornar_Sucesso()
        {
            var response = _service.AdicionarNovoPorcesso(3);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Accepted);
            Assert.AreNotEqual(response.IdProcesso, new Guid());
            Assert.IsTrue(response.IsValid);
            Assert.IsTrue(response.MensagensDeErro.Count == 0);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void ObterProcessoPor_Deverar_Retornar_Sucesso()
        {
            var response = _service.ObterProcessoPor(Guid.NewGuid().ToString());

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(response.Processo);
            Assert.IsTrue(response.IsValid);
            Assert.IsTrue(response.MensagensDeErro.Count == 0);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void ObterTodosProcessos_Deverar_Retornar_Sucesso()
        {
            var response = _service.ObterTodosProcessos();

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(response.Processos);
            Assert.IsTrue(response.Processos.Count > 0);
            Assert.IsTrue(response.IsValid);
            Assert.IsTrue(response.MensagensDeErro.Count == 0);
        }


        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void ObterImagemProcessoPor_Deve_Retornar_Imagem()
        {
            // Arrange
            var id = Guid.NewGuid();
            var bitMap = new Bitmap(1, 1);
            _mockDesignerService.Setup(it => it.Inicializar(It.IsAny<global::Infrastructure.TorreHanoi.ImagemHelper.Dto.TorreHanoiDto>()));
            _mockDesignerService.Setup(it => it.Desenhar()).Returns(bitMap);

            // Act
            var response = _service.ObterImagemProcessoPor(id.ToString());

            // Assert
            _mockTorreHanoiDomainService.Verify(it => it.ObterPor(id), Times.Once);
            Assert.IsNotNull(response.Imagem);
            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(response.Imagem.Height, 1);
            Assert.AreEqual(response.Imagem.Width, 1);
        }

        private global::Domain.TorreHanoi.TorreHanoi CriarTorreHanoi() => new global::Domain.TorreHanoi.TorreHanoi(3, _mockLogger.Object);

    }
}
