namespace BarberSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BARBER_DATABASE : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AGENDA",
                c => new
                    {
                        codigo = c.Int(nullable: false, identity: true),
                        cliente = c.String(maxLength: 50, unicode: false),
                        hora_inicio = c.DateTime(),
                        hora_fim = c.DateTime(),
                        data = c.DateTime(storeType: "date"),
                        codcliente = c.Int(),
                        codbarbeiro = c.Int(),
                        nome_barbeiro = c.String(maxLength: 50, unicode: false),
                        descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.codigo)
                .ForeignKey("dbo.BARBEIROS", t => t.codbarbeiro)
                .ForeignKey("dbo.CLIENTES", t => t.codcliente)
                .Index(t => t.codcliente)
                .Index(t => t.codbarbeiro);
            
            CreateTable(
                "dbo.BARBEIROS",
                c => new
                    {
                        codigo = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, maxLength: 50, unicode: false),
                        endereco = c.String(nullable: false, maxLength: 50, unicode: false),
                        bairro = c.String(nullable: false, maxLength: 30, unicode: false),
                        numero = c.Int(),
                        cidade = c.String(nullable: false, maxLength: 30, unicode: false),
                        cep = c.String(nullable: false, maxLength: 20, unicode: false),
                        celular = c.String(nullable: false, maxLength: 20, unicode: false),
                        sexo = c.String(nullable: false, maxLength: 15, unicode: false),
                        estado = c.String(nullable: false, maxLength: 2, unicode: false),
                    })
                .PrimaryKey(t => t.codigo);
            
            CreateTable(
                "dbo.CLIENTES",
                c => new
                    {
                        codigo = c.Int(nullable: false, identity: true),
                        nome = c.String(maxLength: 50, unicode: false),
                        endereco = c.String(maxLength: 50, unicode: false),
                        bairro = c.String(maxLength: 30, unicode: false),
                        cidade = c.String(maxLength: 30, unicode: false),
                        estado = c.String(maxLength: 2, unicode: false),
                        cep = c.String(nullable: false, maxLength: 20, unicode: false),
                        numero = c.Int(),
                        telefone = c.String(nullable: false, maxLength: 20, unicode: false),
                        celular = c.String(nullable: false, maxLength: 20, unicode: false),
                        sexo = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.codigo);
            
            CreateTable(
                "dbo.CAIXA",
                c => new
                    {
                        codigo = c.Int(nullable: false, identity: true),
                        entrada = c.Double(),
                        saida = c.Double(),
                        vl_total = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.codigo);
            
            CreateTable(
                "dbo.CONTAS_PAGAR",
                c => new
                    {
                        codigo = c.Int(nullable: false, identity: true),
                        descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                        data_vencto = c.DateTime(),
                        data_pagto = c.DateTime(),
                        vl_unitario = c.Double(),
                        vl_total = c.Double(),
                    })
                .PrimaryKey(t => t.codigo);
            
            CreateTable(
                "dbo.CONTAS_RECEBER",
                c => new
                    {
                        codigo = c.Int(nullable: false, identity: true),
                        descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                        data_vencto = c.DateTime(),
                        data_pagto = c.DateTime(),
                        vl_unitario = c.Double(),
                        vl_total = c.Double(),
                    })
                .PrimaryKey(t => t.codigo);
            
            CreateTable(
                "dbo.ESTOQUE",
                c => new
                    {
                        codigo = c.Int(nullable: false, identity: true),
                        produto = c.String(maxLength: 50, unicode: false),
                        quantidade = c.Int(),
                        vl_produto = c.Double(nullable: false),
                        codproduto = c.Int(),
                        vl_total = c.Double(),
                    })
                .PrimaryKey(t => t.codigo)
                .ForeignKey("dbo.PRODUTOS", t => t.codproduto)
                .Index(t => t.codproduto);
            
            CreateTable(
                "dbo.PRODUTOS",
                c => new
                    {
                        codigo = c.Int(nullable: false, identity: true),
                        descricao = c.String(maxLength: 50, unicode: false),
                        vl_unitario = c.Double(),
                        codfornecedor = c.Int(),
                        nome_fornecedor = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.codigo)
                .ForeignKey("dbo.FORNECEDORES", t => t.codfornecedor)
                .Index(t => t.codfornecedor);
            
            CreateTable(
                "dbo.FORNECEDORES",
                c => new
                    {
                        codigo = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, maxLength: 50, unicode: false),
                        endereco = c.String(nullable: false, maxLength: 50, unicode: false),
                        bairro = c.String(nullable: false, maxLength: 30, unicode: false),
                        numero = c.Int(),
                        cidade = c.String(nullable: false, maxLength: 30, unicode: false),
                        cep = c.String(nullable: false, maxLength: 20, unicode: false),
                        telefone = c.String(nullable: false, maxLength: 20, unicode: false),
                        tipo = c.String(nullable: false, maxLength: 15, unicode: false),
                        estado = c.String(nullable: false, maxLength: 2, unicode: false),
                    })
                .PrimaryKey(t => t.codigo);
            
            CreateTable(
                "dbo.FUNCIONARIOS",
                c => new
                    {
                        codigo = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, maxLength: 50, unicode: false),
                        endereco = c.String(nullable: false, maxLength: 50, unicode: false),
                        bairro = c.String(nullable: false, maxLength: 30, unicode: false),
                        numero = c.Int(),
                        cidade = c.String(nullable: false, maxLength: 30, unicode: false),
                        cep = c.String(nullable: false, maxLength: 20, unicode: false),
                        telefone = c.String(nullable: false, maxLength: 20, unicode: false),
                        celular = c.String(nullable: false, maxLength: 20, unicode: false),
                        cargo = c.String(nullable: false, maxLength: 20),
                        estado = c.String(nullable: false, maxLength: 2, unicode: false),
                        sexo = c.String(nullable: false, maxLength: 15, unicode: false),
                        salario = c.Double(),
                    })
                .PrimaryKey(t => t.codigo);
            
            CreateTable(
                "dbo.USUARIOS",
                c => new
                    {
                        codigo = c.Int(nullable: false, identity: true),
                        nome_usuario = c.String(maxLength: 15, unicode: false),
                        senha = c.String(maxLength: 50, unicode: false),
                        endereco = c.String(maxLength: 50, unicode: false),
                        bairro = c.String(maxLength: 50, unicode: false),
                        cidade = c.String(maxLength: 30, unicode: false),
                        estado = c.String(maxLength: 2, unicode: false),
                        cpf = c.String(nullable: false, maxLength: 15, unicode: false),
                        email = c.String(nullable: false, maxLength: 30, unicode: false),
                        tipo = c.String(maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.codigo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PRODUTOS", "codfornecedor", "dbo.FORNECEDORES");
            DropForeignKey("dbo.ESTOQUE", "codproduto", "dbo.PRODUTOS");
            DropForeignKey("dbo.AGENDA", "codcliente", "dbo.CLIENTES");
            DropForeignKey("dbo.AGENDA", "codbarbeiro", "dbo.BARBEIROS");
            DropIndex("dbo.PRODUTOS", new[] { "codfornecedor" });
            DropIndex("dbo.ESTOQUE", new[] { "codproduto" });
            DropIndex("dbo.AGENDA", new[] { "codbarbeiro" });
            DropIndex("dbo.AGENDA", new[] { "codcliente" });
            DropTable("dbo.USUARIOS");
            DropTable("dbo.FUNCIONARIOS");
            DropTable("dbo.FORNECEDORES");
            DropTable("dbo.PRODUTOS");
            DropTable("dbo.ESTOQUE");
            DropTable("dbo.CONTAS_RECEBER");
            DropTable("dbo.CONTAS_PAGAR");
            DropTable("dbo.CAIXA");
            DropTable("dbo.CLIENTES");
            DropTable("dbo.BARBEIROS");
            DropTable("dbo.AGENDA");
        }
    }
}
