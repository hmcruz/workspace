       IDENTIFICATION DIVISION.
       PROGRAM-ID. SZEMB192.
      ******************************************************************
      * SMART - SISTEMA DE ADMINISTRACAO DE CONTRATOS DE SEGURO DE     *
      *         CREDITO FINANCEIRO.                                    *
      *----------------------------------------------------------------*
      * PROGRAMA : SZEMB192                                            *
      * DESCRICAO: PROCESSAR A PREVIA FINANCEIRA DO FIES               *
      *                                                                *
      *----------------------------------------------------------------*
      * HISTORICO DE CORRECOES                                         *
      *----------------------------------------------------------------*
      * NO DATA    RESPONSAVEL         ALTERACAO                       *
      *----------------------------------------------------------------*
      * 2 10/06/2021 FELIPE TOGAWA    AJUSTE LAYOUT FIES               *
      *   MOTIVO: JAZZ 288841                                          *
      *   VERSAO: 2                   PROCURE V.02                     *
      ******************************************************************
      * 1 02/03/2021 Rildo             CODE                            *
      ******************************************************************
       ENVIRONMENT DIVISION.
       CONFIGURATION SECTION.
      *SPECIAL-NAMES.
      *    DECIMAL-POINT IS COMMA.
       INPUT-OUTPUT                   SECTION.
      *----------------------------------------------------------------*
       DATA DIVISION.
      *----------------------------------------------------------------*
       WORKING-STORAGE                SECTION.
      *----------------------------------------------------------------*
       77 WS-PROGRAMA-VERSAO               PIC  X(060) VALUE
               'SZEMB192-CARGA ARQUIVO FINANCEIRO FIES-V20210303-17H00'.
       77 WS-PROGRAMA                      PIC  X(008) VALUE 'SZEMB192'.
       77 WS-COD-RETORNO-0101              PIC  9(005) VALUE 0.
       77 WS-COD-RETORNO-SLD               PIC  9(005) VALUE 0.
       77 WS-CALL                          PIC  X(008) VALUE SPACES.
       77 WS-COMMIT                        PIC  9(009) VALUE 80000.
       77 WS-HRA-GER-MOV                   PIC  X(008) VALUE SPACES.
       77 WS-COUNT                         PIC S9(004) COMP.
       77 WS-COUNTR                        PIC S9(004) COMP.
       77 WS-NULL                          PIC S9(004) COMP.
       77 WS-DTA-SOLICITA-NULL             PIC S9(004) COMP.
       77 WS-DTA-RETORNO-NULL              PIC S9(004) COMP.
       77 WS-DTA-ENVIO-REL-NULL            PIC S9(004) COMP.
       77 WS-DTA-ENVIO-SIGPF-NULL          PIC S9(004) COMP.
       77 WS-DTA-ENVIO-SIGDC-NULL          PIC S9(004) COMP.
       77 WS-DTA-ENVIO-SMS-NULL            PIC S9(004) COMP.
       77 WS-SEQ-AVISO-NULL                PIC S9(004) COMP.
       77 WS-COD-TAXA-PREMIO-NULL          PIC S9(004) COMP VALUE +0.
       77 WS-VLR-PRM-CALC-NULL             PIC S9(004) COMP VALUE +0.
       77 WS-VLR-PRM-CALC-IOF-NULL         PIC S9(004) COMP VALUE +0.
       77 WS-VLR-PRM-ESTIP-NULL            PIC S9(004) COMP VALUE +0.
       77 WS-VLR-PRM-ESTIP-IOF-NULL        PIC S9(004) COMP VALUE +0.
       77 WS-VLR-PRM-CORR-NULL             PIC S9(004) COMP VALUE +0.
       77 WS-VLR-PRM-CORR-IOF-NULL         PIC S9(004) COMP VALUE +0.
       77 WS-COD-EVENTO-NULL               PIC S9(004) COMP VALUE +0.
       77 WS-DTA-IDLG-NULL                 PIC S9(004) COMP.
       77 WS-VLR-NUM                       PIC S9(018) USAGE COMP.
       77 WS-VLR-NUM-DEC                   PIC S9(013)V99 USAGE COMP-3.
       77 WS-CREDITO-DISPONIVEL            PIC S9(013)V99 USAGE COMP-3.
       77 WS-NUM-ITEM                      PIC  9(007) VALUE ZEROS.
       77 WS-MAX-ITEM                      PIC  9(007) VALUE ZEROS.
       77 WS-REJEITADO                     PIC  X(001) VALUE 'N'.
       77 WS-CRITICA                       PIC  X(060) VALUE SPACES.
       77 WS-CONTEUDO                      PIC  X(018).
       77 WS-COD-ATRIBUTO                  PIC  X(018).
       77 WS-COD-CRITICA                   PIC  9(004).
       77 WS-SEQ-MOV-CRITICA               PIC  9(005) VALUE ZEROS.
       77 WS-TAM                           PIC  9(006) VALUE ZEROS.
       77 WS-DTA-INICIO-CONTRATO           PIC  X(010) VALUE SPACES.
       77 WS-PRAZO-CONTRATO                PIC S9(004) COMP.
       77 WS-CALC-IOF                      PIC  9(12)V9(02) VALUE ZEROS.
       77 WS-IND-ISENTO-IOF                PIC  X(01).
       77 WS-FIM-CURSOR1                   PIC S9(004) COMP VALUE +0.
       77 WS-FIM-CURSOR2                   PIC S9(004) COMP VALUE +0.

      *-----------------------------------------------------------------
      *- DECLARA VARIAVEIS DE TRABALHO - VARIAVEIS PARA EDICAO
      *-----------------------------------------------------------------

       77  W-SQLCODE-ED                  PIC --999.
       77  W-COD-PRODUTO-CVP-ED          PIC 9999 VALUE 0.
       77  W-COD-PRODUTO-JV1-ED          PIC 9999 VALUE 0.
       77  W-NUM-APOLICE-CVP-ED          PIC 9(015) VALUE 0.
       77  W-NUM-APOLICE-JV1-ED          PIC 9(015) VALUE 0.
       77  W-SMALLINT-ED1                PIC -----9.
       77  W-SMALLINT-ED2                PIC -----9.
       77  W-SMALLINT-ED3                PIC -----9.
       77  W-SMALLINT-ED4                PIC -----9.
       77  W-INTEGER-ED1                 PIC ---------9.
       77  W-INTEGER-ED2                 PIC ---------9.
       77  W-INTEGER-ED3                 PIC ---------9.
       77  W-INTEGER-ED4                 PIC ---------9.
       77  W-BIGINT-ED1                  PIC ------------------9.
       77  W-BIGINT-ED2                  PIC ------------------9.
       77  W-DECIMAL-ED13V0              PIC --------------9.
       77  W-DECIMAL-ED15V0              PIC ---------------9.
       77  W-DECIMAL-ED15V1              PIC ---------------9.

      *-----------------------------------------------------------------
      *- DECLARA VARIAVEIS HOSTS INDICADORAS DE NULOS
      *-----------------------------------------------------------------
       77 VN-NUM-CONTRATO                  PIC S9(004) COMP.
       77 VN-SEQ-PREMIO                    PIC S9(004) COMP.
      *
      *--- AREA DO ARQUIVO DE PARAMETROS
      *
       01 WS-REG-PARAMETROS.
          03 WS-DT-MOVIMENTO.
             05  WS-DT-ANO-MOVIMENTO       PIC  9(004).
             05  WS-DT-MES-MOVIMENTO       PIC  9(002).
             05  WS-DT-DIA-MOVIMENTO       PIC  9(002).
          03 WS-NOM-ARQUIVO                PIC  X(040).
      *
       01 WS-STATUS                        PIC  X(002) VALUE '00'.
       01 WS-FIM-ARQUIVO                   PIC  X(001) VALUE SPACES.
       01 CNT-CONTADORES.
          03  CNT-LIDOS1                   PIC  9(009) VALUE 0.
          03  CNT-LIDOS2                   PIC  9(009) VALUE 0.
          03  CNT-HEADER                   PIC  9(009) VALUE 0.
          03  CNT-GRAVADOS                 PIC  9(009) VALUE 0.
          03  CNT-DUPLICADOS               PIC  9(009) VALUE 0.
          03  CNT-REJEITADOS               PIC  9(009) VALUE 0.
          03  CNT-CONTR-FINANC             PIC  9(009) VALUE 0.
          03  CNT-QT-PREMIO                PIC  9(009) VALUE ZEROS.
      *
      *---- AREA COMUM PARA REGISTRAR ERROS
       01  WS-AREA-ERROS-XXX.
           03 WS-IND-ERRO-XXX            PIC  9(005).
           03 WS-MSG-RET-XXX             PIC  X(133).
           03 WS-NM-TAB-XXX              PIC  X(030).
           03 WS-SQLCODE-COMPL-XXX.
              05 WS-SQLCODE-XXX          PIC  ZZZ999-.
           03 WS-SQLERRMC-XXX            PIC  X(070).
      *    --- COMPLEMENTOS
           03 WS-CALL-XXX                PIC  X(008).
           03 WS-MSG-RET-COMPL-XXX       PIC  X(133).
           03 WS-PARAGRAFO-XXX           PIC  X(040).
           03 WS-RETURN-CODE-XXX         PIC  9(005).
      *
       01 WS-IC-NULO.
          03  I-SZ177-NUM-CANAL-RECEBIDO   PIC S9(004) COMP VALUE +0.
          03  I-SZ177-NUM-CANAL-ATRIBUIDO  PIC S9(004) COMP VALUE +0.
          03  I-SZ177-VLR-CONTRATO         PIC S9(004) COMP VALUE +0.
          03  I-SZ177-VLR-SEGURO           PIC S9(004) COMP VALUE +0.
          03  I-SZ177-NUM-PV-RENOVACAO     PIC S9(004) COMP VALUE +0.
      *
       01 WS-VARIAVEIS.
          03  WS-TEM-CONTRATO              PIC X(003)  VALUE SPACES.
          03  WS-TEM-PREMIO                PIC X(003)  VALUE SPACES.
          03  WS-TEM-PREVIA                PIC X(003)  VALUE SPACES.
          03  WS-TEM-PREVIA-HIST           PIC X(003)  VALUE SPACES.
          03  WS-ERRO                      PIC  -999.
          03  WS-SQLCODE                   PIC  ------999.
          03  WS-SQLERRMC                  PIC  X(070) VALUE SPACES.
          03  WS-PARAGRAFO                 PIC  X(005) VALUE SPACES.
          03  WS-DTA-MOVTO                 PIC  X(010).
          03  WS-DT-MOVIMENTO-X10          PIC  X(010) VALUE SPACES.
          03  WS-DT-MOVIMENTO-INI          PIC  X(010) VALUE SPACES.
          03  WS-DT-MOVIMENTO-FIM          PIC  X(010) VALUE SPACES.
          03  WS-DT-FINANCEIRO             PIC  X(010) VALUE SPACES.
          03  WS-CPF                       PIC  X(011).
          03  WS-VLR-SEGURO-ED             PIC  ZZZZZZZZZ9.99.
          03  WS-NUM-CONTRATO              PIC  9(018).
          03  WS-COD-PRODUTO               PIC  9(004).
          03  WS-VLR-SEG-MIP               PIC  9(013)V99.
          03  WS-VLR-SEG-MIP-ALF           REDEFINES WS-VLR-SEG-MIP
                                           PIC  X(015).
          03  WS-VLR-SEG-MIP1              PIC  9(013)V99.
          03  WS-VLR-SEG-MIP-ALF1          REDEFINES WS-VLR-SEG-MIP1.
              05  WS-VLR-SEG-INT           PIC  9(013).
              05  WS-VLR-SEG-DEC           PIC  9(002).

          03  POS                          PIC  9(002).
          03  POS1                         PIC  9(002).
          03  WS-NUM-EXTRATO-N             PIC  9(009).
          03  WS-NUM-EXTRATO-INT           PIC S9(009) COMP.
      *
       01 WS-SZ015-TXT-CONTD-DADOS.
          03  WS-IND-REGISTRO              PIC  X(002).
          03  WS-DATA-MOV                  PIC  X(008).
          03  WS-IND-FORMA-RECEBIMENTO     PIC  X(001).
          03  WS-COD-SEGURADORA            PIC  X(003).
          03  WS-EST-CONTRATO              PIC  X(018).
          03  WS-NUM-EXTRATO               PIC  X(004).
          03  WS-VLR-SEGURO                PIC  X(017).

          COPY SZ02022K.
          COPY SZEMW999.
          COPY SZ02025K.

      *----------------------------------------------------------------*
      * TABELA ERROS DE ATRIBUTOS
      *----------------------------------------------------------------*
      * TRATAMENTO DE ERRO
           EXEC SQL INCLUDE SQLCA    END-EXEC.
      * SZ_CRITICA
           EXEC SQL INCLUDE SZ003    END-EXEC.
      * SZ_CONTR_TERC
           EXEC SQL INCLUDE SZ012    END-EXEC.
      * SZ_MOV_CONTROLE
           EXEC SQL INCLUDE SZ014    END-EXEC.
      * SZ_MOV_ITEM
           EXEC SQL INCLUDE SZ015    END-EXEC.
      * SZ_MOV_CRITICA
           EXEC SQL INCLUDE SZ016    END-EXEC.
      *SZ_TP_ARQUIVO
           EXEC SQL INCLUDE SZ036    END-EXEC.
      *SZ_PREMIO
           EXEC SQL INCLUDE SZ085    END-EXEC.
      * SZ_CONTR_TERC_PRS
           EXEC SQL INCLUDE SZ186    END-EXEC.
      * SZ_PREVIA_FINANCEIRA
           EXEC SQL INCLUDE SZ250    END-EXEC.
      * SZ_PREVIA_FINAN_HIST
           EXEC SQL INCLUDE SZ251    END-EXEC.
      *----------------------------------------------------------------*
      * CURSOR MOVIMENTO DA PREVIA FINANCEIRO RECEBIDO
      *
           EXEC SQL DECLARE C00 CURSOR WITH HOLD FOR
V.02          SELECT
V.02            2                           AS NUM_PES_OPERADOR
V.02          , 2                           AS NUM_LINHA_PRODUTO
V.02          , (CASE WHEN SUBSTR(I.TXT_CONTD,15,1) = '0'
V.02             THEN BIGINT(SUBSTR(I.TXT_CONTD,16,17))
V.02             ELSE BIGINT(SUBSTR(I.TXT_CONTD,15,18))
V.02             END)                        AS NUM_CONTRATO_TERC
V.02          , SMALLINT(SUBSTR(I.TXT_CONTD,33,04))   AS NUM_PARCELA
V.02          , SUBSTR(I.TXT_CONTD,11,01)   AS IND_FORMA_RECEBIMENTO
V.02          , I.SEQ_RECEBIMENTO
V.02          , I.NUM_ITEM_MOV
V.02          , date(to_date(SUBSTR(I.TXT_CONTD,3,8),'YYYY-MM-DD'))
V.02                                        AS DTA_MOVIMENTO
V.02          , SMALLINT(SUBSTR(I.TXT_CONTD,12,03))
V.02                                        AS COD_BANCO
V.02          , NULLIF(1,1)                 AS NUM_CONTRATO
V.02          , FLOAT(substr(I.TXT_CONTD,49,3)||
V.02              '.'||substr(I.TXT_CONTD,52,2))
V.02                                        AS VLR_PREMIO
V.02          ,(CASE WHEN (SUBSTR(I.TXT_CONTD,54,8) = '00000000')
TGW            THEN date('0001-01-01')
TGW            ELSE date(to_date(SUBSTR(I.TXT_CONTD,54,8),'YYYY-MM-DD'))
TGW            END)   AS DTA_VENCIMENTO
V.02          , FLOAT(substr(I.TXT_CONTD,62,15)||
V.02              '.'||substr(I.TXT_CONTD,77,2))
V.02                                        AS VLR_SALDO_DEV
V.02          , BIGINT(SUBSTR(I.TXT_CONTD,79,14)) AS NUM_CPF
V.02          FROM SEGUROS.SZ_MOV_ITEM I
V.02          JOIN SEGUROS.SZ_MOV_CONTROLE C
V.02            ON C.SEQ_RECEBIMENTO      = I.SEQ_RECEBIMENTO
V.02           AND C.NOM_ARQUIVO          = :WS-NOM-ARQUIVO
V.02           AND C.COD_TP_ARQUIVO       = 'FIN'
V.02           AND SUBSTR(I.TXT_CONTD,19,1) BETWEEN  '0' AND '9'
           END-EXEC.
      *
      *----------------------------------------------------------------*
       PROCEDURE DIVISION.
      *----------------------------------------------------------------*
           PERFORM P0000-INICIAR
           PERFORM DB930-OPEN-C00
           PERFORM DB940-FETCH-C00
           PERFORM P1000-CARREGAR-ARQ-FINAN
             UNTIL WS-FIM-CURSOR1 = 100
           PERFORM DB950-CLOSE-C00
           PERFORM P9000-FINALIZAR
           GOBACK
           .
      *
      *----------------------------------------------------------------*
       P0000-INICIAR.
      *----------------------------------------------------------------*
           MOVE 'P0000'                  TO WS-PARAGRAFO
           INITIALIZE WS-MSG-OCORR WS-NUM-ITEM
                      LINKAGE-SZ02025S
                      WS-SEQ-MOV-CRITICA
                      REPLACING ALPHANUMERIC BY SPACES
                                     NUMERIC BY ZEROES
           MOVE WS-PROGRAMA    TO WS-COD-OCORRENCIA
           DISPLAY '                                   '
           DISPLAY '***********************************'
           DISPLAY '*           SZEMB192              *'
           DISPLAY '*  PROCESSA ARQUIVO FINANCEIRO    *'
           DISPLAY '*           DO FIES               *'
           DISPLAY '***********************************'
      *     DISPLAY '* VERSAO 01 - INICIO PROCESSAMENTO EM  '
V.02       DISPLAY '* VERSAO 02 - INICIO PROCESSAMENTO EM  '
                     FUNCTION CURRENT-DATE(07:2) '/'
                     FUNCTION CURRENT-DATE(05:2) '/'
                     FUNCTION CURRENT-DATE(01:4) ' AS '
                     FUNCTION CURRENT-DATE(09:2) ':'
                     FUNCTION CURRENT-DATE(11:2) ':'
                     FUNCTION CURRENT-DATE(13:2)
           MOVE 'I'            TO WS-TP-OCORRENCIA
           STRING '* INICIO ' WS-PROGRAMA-VERSAO
                  DELIMITED BY SIZE
                       INTO WS-MSG
           END-STRING
           MOVE WS-MSG-OCORR   TO LK-ENTRADA-2022
           PERFORM P8000-REGISTRAR-OCORRENCIA
           INITIALIZE WS-VARIAVEIS CNT-CONTADORES WS-ERRO
                      DCLSZ-PREMIO
      *
      * Inicia abertura do arquivo de parametros
           ACCEPT WS-REG-PARAMETROS FROM SYSIN
      *
           STRING
                   WS-DT-ANO-MOVIMENTO
                   '-'
                   WS-DT-MES-MOVIMENTO
                   '-'
                   WS-DT-DIA-MOVIMENTO
                DELIMITED BY SIZE INTO WS-DT-MOVIMENTO-X10
           END-STRING
           MOVE WS-DT-MOVIMENTO-X10 TO WS-DTA-MOVTO
      *
      *------- INICIALIZA AREA COMUM PARA REGISTRAR ERROS
           INITIALIZE WS-IND-ERRO-XXX
                      WS-MSG-RET-XXX
                      WS-NM-TAB-XXX
                      WS-SQLCODE-COMPL-XXX
                      WS-SQLERRMC-XXX
                      WS-CALL-XXX
                      WS-MSG-RET-COMPL-XXX
                      WS-RETURN-CODE-XXX
      *
           DISPLAY '* DATA: ' WS-DT-MOVIMENTO-X10
           DISPLAY '* ARQUIVO: ' WS-NOM-ARQUIVO
           DISPLAY '***********************************'
           .
      *----------------------------------------------------------------*
       P1000-CARREGAR-ARQ-FINAN.
      *----------------------------------------------------------------*
           MOVE 'P1000'                  TO WS-PARAGRAFO

           PERFORM P1100-MONTA-PREVIA
              THRU P1100-MONTA-PREVIA-EXIT
      *
           PERFORM DB010-INS-PREVIA-FINAN
              THRU DB010-INS-PREVIA-FINAN-EXIT

      *    IF WS-TEM-PREVIA = 'SIM'
      *       DISPLAY '* --------------------------- *'
      *       DISPLAY '* PREVIA JA EXISTE PARA:   '
      *       DISPLAY '* SZ250-NUM-PES-OPERADOR  ='
      *                  SZ250-NUM-PES-OPERADOR
      *       DISPLAY '* SZ250-NUM-LINHA-PRODUTO ='
      *                  SZ250-NUM-LINHA-PRODUTO
      *       DISPLAY '* SZ250-NUM-CONTRATO-TERC ='
      *                  SZ250-NUM-CONTRATO-TERC
      *       DISPLAY '* SZ250-NUM-PARCELA       ='
      *                  SZ250-NUM-PARCELA
      *       DISPLAY '* SZ250-IND-FORMA-RECEBIMENTO ='
      *                  SZ250-IND-FORMA-RECEBIMENTO
      *    END-IF
      *
           PERFORM DB020-INS-PREVIA-HIST
              THRU DB020-INS-PREVIA-HIST-EXIT
      *
           PERFORM DB940-FETCH-C00
              THRU DB940-FETCH-C00-EXIT
           .
      *
       P1000-CARREGAR-ARQ-FINAN-EXIT. EXIT.
      *----------------------------------------------------------------*
       P1100-MONTA-PREVIA.
      *----------------------------------------------------------------*
           MOVE SZ250-NUM-PES-OPERADOR  TO SZ012-NUM-PES-OPERADOR
                                           SZ251-NUM-PES-OPERADOR
           MOVE SZ250-NUM-LINHA-PRODUTO TO SZ012-NUM-LINHA-PRODUTO
                                           SZ251-NUM-LINHA-PRODUTO
           MOVE SZ250-NUM-CONTRATO-TERC TO SZ012-NUM-CONTRATO-TERC
                                           SZ251-NUM-CONTRATO-TERC
           MOVE SZ250-NUM-PARCELA       TO SZ085-QTD-PARCELA
                                           SZ251-NUM-PARCELA
           MOVE SZ250-IND-FORMA-RECEBIMENTO
                                        TO SZ251-IND-FORMA-RECEBIMENTO
           MOVE SZ250-SEQ-RECEBIMENTO   TO SZ251-SEQ-RECEBIMENTO
           MOVE SZ250-NUM-ITEM-MOV      TO SZ251-NUM-ITEM-MOV

      *    display 'SZ250-NUM-CONTRATO-TERC = ' SZ250-NUM-CONTRATO-TERC

           PERFORM DB300-RECUPERAR-CONTRATO
              THRU DB300-RECUPERAR-CONTRATO-EXIT
           IF WS-TEM-CONTRATO = 'SIM'
              MOVE SZ012-NUM-CONTRATO  TO SZ250-NUM-CONTRATO
              MOVE 0                   TO VN-NUM-CONTRATO
      *       DISPLAY 'ACHOU CONTRATO ' VN-NUM-CONTRATO
           ELSE
              INITIALIZE                  SZ250-NUM-CONTRATO
              MOVE -1                  TO VN-NUM-CONTRATO
      *       DISPLAY 'NAO ACHOU CONTRATO ' VN-NUM-CONTRATO
           END-IF
      *


      * ---
      * --- SZ_PREVIA_FINANCEIRA
      * --- FORMA DE RECEBIMENTO = R (REPASSE) PREMIO = C (COBRANCA)
      * --- FORMA DE RECEBIMENTO = E (ESTORNO) PREMIO = R (RESTITUICAO)
      * ---
           EVALUATE SZ250-IND-FORMA-RECEBIMENTO
              WHEN 'R' MOVE 'C'        TO SZ085-IND-TP-PREMIO
              WHEN 'E' MOVE 'R'        TO SZ085-IND-TP-PREMIO
              WHEN OTHER
                   STRING 'IND-FORMA-RECEBIMENTO INVALIDO = <<'
                    SZ250-IND-FORMA-RECEBIMENTO
                    '>> *** ERRO ***'
                   DELIMITED BY SIZE INTO WS-MSG
                   MOVE 'E'            TO WS-TP-OCORRENCIA
                   MOVE WS-MSG-OCORR   TO LK-ENTRADA-2022
                   DISPLAY WS-MSG
                   MOVE 001            TO WS-IND-ERRO-XXX
                   MOVE SQLCODE        TO WS-SQLCODE-XXX
                   MOVE SQLERRMC       TO WS-SQLERRMC-XXX
                   MOVE WS-MSG         TO WS-MSG-RET-XXX
                   MOVE 99             TO WS-RETURN-CODE-XXX

                  DISPLAY '0001 - EVALUATE'
                  PERFORM P9999-DB2-ERRO
           END-EVALUATE

           MOVE 'POB'            TO SZ250-STA-MOVIMENTO
           IF WS-TEM-CONTRATO = 'SIM'
              MOVE SZ012-NUM-CONTRATO  TO SZ085-NUM-CONTRATO
              INITIALIZE                  SZ250-SEQ-PREMIO
              MOVE -1                  TO VN-SEQ-PREMIO

              PERFORM DB400-RECUPERAR-PREMIO
                 THRU DB400-RECUPERAR-PREMIO-EXIT

              IF WS-TEM-PREMIO = 'SIM'
                 MOVE 'EMT'            TO SZ250-STA-MOVIMENTO
                 MOVE SZ085-SEQ-PREMIO TO SZ250-SEQ-PREMIO
                 MOVE 0                TO VN-SEQ-PREMIO
              END-IF
           END-IF
      *
           MOVE WS-PROGRAMA            TO SZ250-COD-USUARIO
                                          SZ250-COD-PROGRAMA
           .
       P1100-MONTA-PREVIA-EXIT. EXIT.
      *----------------------------------------------------------------*
       P8000-REGISTRAR-OCORRENCIA.
      *----------------------------------------------------------------*
           MOVE ZEROS TO LK-RETORNO-2022
           MOVE 'SZ02022S' TO WS-CALL
           CALL WS-CALL USING LINKAGE-SZ02022S
           END-CALL
           IF LK-RETORNO-2022 NOT EQUAL ZEROS
              MOVE LK-RETORNO-2022 TO WS-COD-RETORNO-0101
              STRING 'ERRO: SZ02022S RETORNO:' LK-RETORNO-2022
                     ' ENCERRADO.'
                     DELIMITED BY SIZE INTO WS-MSG
              END-STRING
              MOVE LK-RETORNO-2022 TO WS-COD-RETORNO-0101
              MOVE 'P7000'         TO WS-LOCAL
              MOVE 'E'            TO WS-TP-OCORRENCIA
              MOVE WS-MSG-OCORR   TO LK-ENTRADA-2022
                 EXEC SQL ROLLBACK END-EXEC
                 DISPLAY WS-MSG
                 DISPLAY ' ROLLBACK EM TUDO'
              STOP RUN
           END-IF
           MOVE SPACES TO WS-DESCR-MSG
           .
      *----------------------------------------------------------------*
       P9999-DB2-ERRO.
      *----------------------------------------------------------------*
V.11  *    MOVE WS-RETURN-CODE-XXX       TO RETURN-CODE
           DISPLAY 'SZEMB192 - PROCESSAMENTO ENCERRADO POR ERRO EM:  '
                   FUNCTION CURRENT-DATE(07:2) '/'
                   FUNCTION CURRENT-DATE(05:2) '/'
                   FUNCTION CURRENT-DATE(01:4) ' AS '
                   FUNCTION CURRENT-DATE(09:2) ':'
                   FUNCTION CURRENT-DATE(11:2) ':'
                   FUNCTION CURRENT-DATE(13:2) ' <<<<<<< '
           DISPLAY 'SZEMB192 - WS-IND-ERRO-XXX    ' WS-IND-ERRO-XXX
           DISPLAY 'SZEMB192 - WS-SQLCODE-XXX     ' WS-SQLCODE-XXX
           DISPLAY 'SZEMB192 - WS-SQLERRMC-XXX    ' WS-SQLERRMC-XXX
           DISPLAY 'SZEMB192 - WS-MSG-RET-XXX     ' WS-MSG-RET-XXX
           DISPLAY 'SZEMB192 - WS-RETURN-CODE-XXX ' WS-RETURN-CODE-XXX
           DISPLAY
           '==========================================================='
           MOVE 'SZEMB192'           TO WS-COD-OCORRENCIA
           MOVE 'E'                  TO WS-TP-OCORRENCIA
           PERFORM P9400-ROLLBACK
           IF WS-IND-ERRO-XXX  NOT EQUAL ZEROS AND
              WS-IND-ERRO-XXX  NOT EQUAL 100
              PERFORM P8000-REGISTRAR-OCORRENCIA
           END-IF
V.11       MOVE WS-RETURN-CODE-XXX   TO RETURN-CODE
           STOP RUN
           .
      *----------------------------------------------------------------*
       P9000-FINALIZAR.
      *----------------------------------------------------------------*
           MOVE 'P9000'                  TO WS-PARAGRAFO
           DISPLAY '                                   '
           DISPLAY '*************************************'
           DISPLAY '** SZEMB192 - PROCESSAMENTO NORMAL **'
           DISPLAY '*************************************'
           DISPLAY '*                                   *'
           DISPLAY '* QTD. LIDOS ARQ.    : ' CNT-LIDOS1       '    *'
           DISPLAY '* QTD. LIDOS MOV.    : ' CNT-LIDOS2       '    *'
           DISPLAY '* GRAVADOS                          *'
           DISPLAY '* PREMIO......:' CNT-QT-PREMIO   '            *'
           DISPLAY '* CONTR-FINANC:' CNT-CONTR-FINANC '            *'
           DISPLAY '*                                   *'
           DISPLAY '*************************************'
           EXEC SQL COMMIT END-EXEC
      *      EXEC SQL ROLLBACK END-EXEC
           PERFORM P9901-DISPLAY-FINAL
           .
      *----------------------------------------------------------------*
       P9400-ROLLBACK.
      *----------------------------------------------------------------*
           EXEC SQL ROLLBACK END-EXEC
           DISPLAY ' ROLLBACK P9400'
           IF SQLCODE NOT EQUAL ZEROS
              MOVE SQLCODE TO WS-COD-RETORNO-0101 WS-SQLCODE
              STRING 'ERRO: ROLLBACK ' WS-COD-RETORNO-0101
                     ' - ENCERRADO'
                DELIMITED BY SIZE INTO WS-MSG
              END-STRING
              MOVE 'P9400'        TO WS-LOCAL
              MOVE 'E'            TO WS-TP-OCORRENCIA
              MOVE WS-MSG-OCORR   TO LK-ENTRADA-2022
              DISPLAY WS-MSG-OCORR
              STOP RUN
           END-IF
           .
      *----------------------------------------------------------------*
       P9900-CANCELAR-PROGRAMA.
      *----------------------------------------------------------------*
           DISPLAY '                                   '
           DISPLAY '* SZEMB192 - MOVIMENTO FINANCEIRO *'
           DISPLAY '***********************************'
           DISPLAY '*       >>>>> ATENCAO <<<<<       *'
           DISPLAY '*                                 *'
           DISPLAY '*       ERRO NO PROCESSAMENTO     *'
           DISPLAY '*                                 *'
           DISPLAY '*       >>>>> ATENCAO <<<<<       *'
           DISPLAY '***********************************'
           DISPLAY 'NUM-CONTRATO   ==> ' WS-NUM-CONTRATO
      *    DISPLAY 'CPF            ==> ' WS-ED-CPF
           DISPLAY 'PARAGRAFO      ==> ' WS-PARAGRAFO
           DISPLAY 'STATUS ARQUIVO ==> ' WS-STATUS
           DISPLAY 'SQLCODE        ==> ' WS-SQLCODE
           DISPLAY 'SQLERRMC       ==> ' WS-SQLERRMC
           DISPLAY '                                   '
           PERFORM P9901-DISPLAY-FINAL
           EXEC SQL ROLLBACK END-EXEC
           MOVE 99                         TO RETURN-CODE
           GOBACK
           .
      *
      *----------------------------------------------------------------*
       P9901-DISPLAY-FINAL.
      *----------------------------------------------------------------*
           DISPLAY '* --------------------------------- *'
           DISPLAY '* DATA MOVIMENTO     : ' WS-DT-MOVIMENTO-X10(9:2)
                                     '-' WS-DT-MOVIMENTO-X10(6:2)
                                     '-' WS-DT-MOVIMENTO-X10(1:4) '   *'
           DISPLAY '* QTD. LIDOS ARQ.    : ' CNT-LIDOS1       '    *'
           DISPLAY '* QTD. LIDOS MOV.    : ' CNT-LIDOS2       '    *'
           DISPLAY '* QTD. HEADER        : ' CNT-HEADER      '    *'
           DISPLAY '* QTD. CONTR FINANC  : ' CNT-CONTR-FINANC '    *'
           DISPLAY '* QTD. REJEITADOS    : ' CNT-REJEITADOS  '    *'
           DISPLAY '* --------------------------------- *'
           DISPLAY '* SZEMB192 - FINAL DO PROCESSAMENTO EM '
                     FUNCTION CURRENT-DATE(07:2) '/'
                     FUNCTION CURRENT-DATE(05:2) '/'
                     FUNCTION CURRENT-DATE(01:4) ' AS '
                     FUNCTION CURRENT-DATE(09:2) ':'
                     FUNCTION CURRENT-DATE(11:2) ':'
                     FUNCTION CURRENT-DATE(13:2)
           .
       P9901-DISPLAY-FINAL-EXIT. EXIT.
      *----------------------------------------------------------------*
       DB010-INS-PREVIA-FINAN.
      *----------------------------------------------------------------*
           MOVE 'NAO'                  TO WS-TEM-PREVIA
           EXEC SQL
                INSERT INTO SEGUROS.SZ_PREVIA_FINANCEIRA (
                      NUM_PES_OPERADOR
                    , NUM_LINHA_PRODUTO
                    , NUM_CONTRATO_TERC
                    , NUM_PARCELA
                    , IND_FORMA_RECEBIMENTO
                    , SEQ_RECEBIMENTO
                    , NUM_ITEM_MOV
                    , DTA_MOVIMENTO
                    , COD_BANCO
                    , NUM_CONTRATO
                    , SEQ_PREMIO
                    , VLR_PREMIO
                    , STA_MOVIMENTO
                    , COD_USUARIO
                    , COD_PROGRAMA
                    , DTH_CADASTRAMENTO
                    , DTA_VENCIMENTO
                    , VLR_SALDO_DEVEDOR
                    , NUM_CPF_CNPJ)
                VALUES(
                      :SZ250-NUM-PES-OPERADOR
                    , :SZ250-NUM-LINHA-PRODUTO
                    , :SZ250-NUM-CONTRATO-TERC
                    , :SZ250-NUM-PARCELA
                    , :SZ250-IND-FORMA-RECEBIMENTO
                    , :SZ250-SEQ-RECEBIMENTO
                    , :SZ250-NUM-ITEM-MOV
                    , :SZ250-DTA-MOVIMENTO
                    , :SZ250-COD-BANCO
                    , :SZ250-NUM-CONTRATO :VN-NUM-CONTRATO
                    , :SZ250-SEQ-PREMIO   :VN-SEQ-PREMIO
                    , :SZ250-VLR-PREMIO
                    , :SZ250-STA-MOVIMENTO
                    , :SZ250-COD-USUARIO
                    , :SZ250-COD-PROGRAMA
                    , CURRENT TIMESTAMP
V.02                , :SZ250-DTA-VENCIMENTO
V.02                , :SZ250-VLR-SALDO-DEVEDOR
V.02                , :SZ250-NUM-CPF-CNPJ)
           END-EXEC

           EVALUATE SQLCODE
              WHEN 000
                   CONTINUE
              WHEN -803
                   MOVE 'SIM'          TO WS-TEM-PREVIA
              WHEN OTHER
                   MOVE SQLCODE    TO WS-ERRO
                                      WS-SQLCODE
                                      WS-COD-RETORNO-0101
                   MOVE SZ250-SEQ-RECEBIMENTO  TO W-INTEGER-ED1
                   MOVE SZ250-NUM-ITEM-MOV     TO W-INTEGER-ED2
                   STRING 'ERRO INSERT SZ_PREVIA_FINANCEIRA '
                     ' SEQ-RECEBIMENTO ='  W-INTEGER-ED1
                     ' NUM-ITEM-MOV='  W-INTEGER-ED2
                     ' SQLCODE:' WS-COD-RETORNO-0101
                     ' SQLERRMC:' SQLERRMC
                     DELIMITED BY SIZE INTO WS-MSG
                   END-STRING
                   MOVE 'E'                 TO WS-TP-OCORRENCIA
                   MOVE 'DB010'             TO WS-LOCAL
                   MOVE WS-MSG-OCORR        TO LK-ENTRADA-2022
                   DISPLAY WS-MSG
                   MOVE 002                 TO WS-IND-ERRO-XXX
                   MOVE SQLCODE             TO WS-SQLCODE-XXX
                   MOVE SQLERRMC            TO WS-SQLERRMC-XXX
                   MOVE WS-MSG              TO WS-MSG-RET-XXX
                   MOVE 99                  TO WS-RETURN-CODE-XXX
                   DISPLAY '002 - INSERT '
                   PERFORM P9999-DB2-ERRO
           END-EVALUATE


           .
       DB010-INS-PREVIA-FINAN-EXIT. EXIT.
      *----------------------------------------------------------------*
       DB020-INS-PREVIA-HIST.
      *----------------------------------------------------------------*
           MOVE 'NAO'                  TO WS-TEM-PREVIA-HIST
           EXEC SQL
                INSERT INTO SEGUROS.SZ_PREVIA_FINAN_HIST (
                      NUM_PES_OPERADOR
                    , NUM_LINHA_PRODUTO
                    , NUM_CONTRATO_TERC
                    , NUM_PARCELA
                    , IND_FORMA_RECEBIMENTO
                    , SEQ_RECEBIMENTO
                    , NUM_ITEM_MOV    )
                VALUES(
                      :SZ251-NUM-PES-OPERADOR
                    , :SZ251-NUM-LINHA-PRODUTO
                    , :SZ251-NUM-CONTRATO-TERC
                    , :SZ251-NUM-PARCELA
                    , :SZ251-IND-FORMA-RECEBIMENTO
                    , :SZ251-SEQ-RECEBIMENTO
                    , :SZ251-NUM-ITEM-MOV)
           END-EXEC

           EVALUATE SQLCODE
              WHEN 000
                   CONTINUE
              WHEN -803
                   MOVE 'SIM'          TO WS-TEM-PREVIA-HIST
              WHEN OTHER
                   MOVE SQLCODE    TO WS-ERRO
                                      WS-SQLCODE
                                      WS-COD-RETORNO-0101
                   MOVE SZ250-SEQ-RECEBIMENTO  TO W-INTEGER-ED1
                   MOVE SZ250-NUM-ITEM-MOV     TO W-INTEGER-ED2
                   STRING 'ERRO INSERT SZ_PREVIA_FINAN_HIST '
                     ' SEQ-RECEBIMENTO ='  W-INTEGER-ED1
                     ' NUM-ITEM-MOV='  W-INTEGER-ED2
                     ' SQLCODE:' WS-COD-RETORNO-0101
                     ' SQLERRMC:' SQLERRMC
                     DELIMITED BY SIZE INTO WS-MSG
                   END-STRING
                   MOVE 'E'                 TO WS-TP-OCORRENCIA
                   MOVE 'DB010'             TO WS-LOCAL
                   MOVE WS-MSG-OCORR        TO LK-ENTRADA-2022
                   DISPLAY WS-MSG
                   MOVE 003                 TO WS-IND-ERRO-XXX
                   MOVE SQLCODE             TO WS-SQLCODE-XXX
                   MOVE SQLERRMC            TO WS-SQLERRMC-XXX
                   MOVE WS-MSG              TO WS-MSG-RET-XXX
                   MOVE 99                  TO WS-RETURN-CODE-XXX
                   DISPLAY '003 - INSERT '
                   PERFORM P9999-DB2-ERRO
           END-EVALUATE
           .
       DB020-INS-PREVIA-HIST-EXIT. EXIT.
      *---------------------------------------------------------------*
       DB300-RECUPERAR-CONTRATO.
      *---------------------------------------------------------------*
           MOVE 'DB300'                TO WS-PARAGRAFO WS-LOCAL
           MOVE ZEROES                 TO WS-ERRO WS-COD-RETORNO-0101
           MOVE 'NAO'                  TO WS-TEM-CONTRATO

           INITIALIZE SZ012-NUM-CONTRATO
      *
           EXEC SQL
                 SELECT SZ012.NUM_CONTRATO
                   INTO :SZ012-NUM-CONTRATO
                 FROM SEGUROS.SZ_CONTR_TERC SZ012
                WHERE SZ012.NUM_CONTRATO_TERC = :SZ012-NUM-CONTRATO-TERC
                  AND SZ012.NUM_PES_OPERADOR  = :SZ012-NUM-PES-OPERADOR
                  AND SZ012.NUM_LINHA_PRODUTO = :SZ012-NUM-LINHA-PRODUTO
                 WITH UR
           END-EXEC
      *
           MOVE SQLCODE  TO WS-ERRO WS-COD-RETORNO-0101
      *
           EVALUATE SQLCODE
              WHEN 000
                   MOVE 'SIM'               TO WS-TEM-CONTRATO
              WHEN 100
                   MOVE 'NAO'               TO WS-TEM-CONTRATO
              WHEN OTHER
                   MOVE SQLCODE  TO WS-ERRO
                                    WS-SQLCODE
                                    WS-COD-RETORNO-0101
                   MOVE SQLERRMC TO WS-SQLERRMC
                   MOVE SZ012-NUM-CONTRATO-TERC  TO WS-NUM-CONTRATO
                   STRING 'ERRO SELECT SZ_CONTRATO_TERC:'
                     WS-NUM-CONTRATO
                     ' - SQLCODE:'     WS-ERRO
                     ' - PROGRAMA:'    WS-PROGRAMA ' ENCERRADO'
                     ' - NOM_ARQUIVO:' WS-NOM-ARQUIVO '.'
                     DELIMITED BY SIZE INTO WS-MSG
                   END-STRING
                   MOVE 'E'                 TO WS-TP-OCORRENCIA
                   MOVE WS-MSG-OCORR        TO LK-ENTRADA-2022
                   DISPLAY WS-MSG
                   MOVE 004                 TO WS-IND-ERRO-XXX
                   MOVE SQLCODE             TO WS-SQLCODE-XXX
                   MOVE SQLERRMC            TO WS-SQLERRMC-XXX
                   MOVE WS-MSG              TO WS-MSG-RET-XXX
                   MOVE 99                  TO WS-RETURN-CODE-XXX

                   DISPLAY '0004 - SZ_CONTR_TERC'
                   PERFORM P9999-DB2-ERRO
           END-EVALUATE
           .
       DB300-RECUPERAR-CONTRATO-EXIT. EXIT.
      *----------------------------------------------------------------*
       DB400-RECUPERAR-PREMIO.
      *----------------------------------------------------------------*
           MOVE ZEROS TO WS-COUNT WS-ERRO WS-SQLCODE
           MOVE 'NAO'                  TO WS-TEM-PREMIO
           INITIALIZE                     SZ085-SEQ-PREMIO
           EXEC SQL
                SELECT SEQ_PREMIO
                 INTO  :SZ085-SEQ-PREMIO
                FROM SEGUROS.SZ_PREMIO SZ085
                WHERE SZ085.NUM_CONTRATO  = :SZ085-NUM-CONTRATO
                  AND SZ085.QTD_PARCELA   = :SZ085-QTD-PARCELA
                  AND SZ085.IND_TP_PREMIO = :SZ085-IND-TP-PREMIO
                  AND SZ085. STA_PREMIO   = 'A'
                WITH UR
           END-EXEC
      *
           EVALUATE SQLCODE
              WHEN 000
                   MOVE 'SIM'               TO WS-TEM-PREMIO
              WHEN 100
                   MOVE 'NAO'               TO WS-TEM-CONTRATO
              WHEN OTHER

           IF SQLCODE NOT EQUAL ZEROS AND 100
              MOVE SQLCODE  TO WS-ERRO WS-SQLCODE WS-COD-RETORNO-0101
              MOVE SQLERRMC TO WS-SQLERRMC
              MOVE SZ012-NUM-CONTRATO-TERC  TO WS-NUM-CONTRATO
              STRING 'ERRO SELECT SZ_CONTRATO_TERC:'
                     WS-NUM-CONTRATO
                     ' - SQLCODE:'     WS-ERRO
                     ' - PROGRAMA:'    WS-PROGRAMA ' ENCERRADO'
                     ' - NOM_ARQUIVO:' WS-NOM-ARQUIVO '.'
                  DELIMITED BY SIZE INTO WS-MSG
              END-STRING
              MOVE 'E'                 TO WS-TP-OCORRENCIA
              MOVE WS-MSG-OCORR        TO LK-ENTRADA-2022
              DISPLAY WS-MSG
              MOVE 005                 TO WS-IND-ERRO-XXX
              MOVE SQLCODE             TO WS-SQLCODE-XXX
              MOVE SQLERRMC            TO WS-SQLERRMC-XXX
              MOVE WS-MSG              TO WS-MSG-RET-XXX
              MOVE 99                  TO WS-RETURN-CODE-XXX

              DISPLAY '0005 - SZ_PREMIO'
              PERFORM P9999-DB2-ERRO
           ELSE
              MOVE 'SIM'                TO WS-TEM-PREMIO
           END-IF
           .
       DB400-RECUPERAR-PREMIO-EXIT. EXIT.
      *----------------------------------------------------------------*
       DB930-OPEN-C00.
      *----------------------------------------------------------------*
      *
           EXEC SQL OPEN C00 END-EXEC
           IF SQLCODE NOT EQUAL ZEROS
              MOVE SQLCODE TO WS-COD-RETORNO-0101
              STRING 'ERRO: OPEN C00-RETORNO:' WS-COD-RETORNO-0101
                     ' SQLERRMC:' SQLERRMC
                DELIMITED BY SIZE INTO WS-MSG
              END-STRING
              MOVE 'E'               TO WS-TP-OCORRENCIA
              MOVE 'DB930'           TO WS-LOCAL
              MOVE WS-MSG-OCORR      TO LK-ENTRADA-2022
      *       PERFORM P8000-FINALIZA

              DISPLAY '* ERRO OPEN C00 ' WS-COD-RETORNO-0101
              MOVE 99 TO RETURN-CODE
              STOP RUN

           END-IF
           .
      *----------------------------------------------------------------*
       DB940-FETCH-C00.
      *----------------------------------------------------------------*
           INITIALIZE  DCLSZ-PREVIA-FINANCEIRA
           EXEC SQL FETCH C00
            INTO :SZ250-NUM-PES-OPERADOR
                ,:SZ250-NUM-LINHA-PRODUTO
                ,:SZ250-NUM-CONTRATO-TERC
                ,:SZ250-NUM-PARCELA
                ,:SZ250-IND-FORMA-RECEBIMENTO
                ,:SZ250-SEQ-RECEBIMENTO
                ,:SZ250-NUM-ITEM-MOV
                ,:SZ250-DTA-MOVIMENTO
                ,:SZ250-COD-BANCO
                ,:SZ250-NUM-CONTRATO :VN-NUM-CONTRATO
                ,:SZ250-VLR-PREMIO
V.02            ,:SZ250-DTA-VENCIMENTO
V.02            ,:SZ250-VLR-SALDO-DEVEDOR
V.02            ,:SZ250-NUM-CPF-CNPJ
           END-EXEC
           MOVE SQLCODE    TO WS-FIM-CURSOR1
           IF SQLCODE NOT EQUAL ZEROS AND 100
              MOVE SQLCODE TO WS-COD-RETORNO-0101
              STRING 'ERRO: FETCH C00 RETORNO:' WS-COD-RETORNO-0101
                     ' SQLERRMC:' SQLERRMC
                DELIMITED BY SIZE INTO WS-MSG
              END-STRING
              MOVE 'E'            TO WS-TP-OCORRENCIA
              MOVE 'DB940'        TO WS-LOCAL
              MOVE WS-MSG-OCORR   TO LK-ENTRADA-2022
      *       PERFORM P8000-FINALIZA
              DISPLAY '* ERRO FETCH C00 ' WS-COD-RETORNO-0101
              DISPLAY ' SQLERRMC: ' SQLERRMC
              MOVE 99 TO RETURN-CODE
              STOP RUN
           END-IF
           IF SQLCODE EQUAL ZEROS
              ADD 1 TO CNT-LIDOS1
           END-IF
           .
       DB940-FETCH-C00-EXIT. EXIT.
      *----------------------------------------------------------------*
       DB950-CLOSE-C00.
      *----------------------------------------------------------------*
           EXEC SQL CLOSE C00 END-EXEC
           IF SQLCODE NOT EQUAL ZEROS
              MOVE SQLCODE TO WS-COD-RETORNO-0101
              STRING 'ERRO: CLOSE C00 RETORNO:' WS-COD-RETORNO-0101
                     ' SQLERRMC:' SQLERRMC
                DELIMITED BY SIZE INTO WS-MSG
              END-STRING
              MOVE 'E'            TO WS-TP-OCORRENCIA
              MOVE 'DB950'        TO WS-LOCAL
              MOVE WS-MSG-OCORR   TO LK-ENTRADA-2022
              DISPLAY '* ERRO CLOSE C00 ' WS-COD-RETORNO-0101
              MOVE 99 TO RETURN-CODE
              STOP RUN
           END-IF
           .
      *----------------------------------------------------------------*
      *    END.
      *----------------------------------------------------------------*