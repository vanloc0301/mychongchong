namespace SkyMap.Net.XMLExchange.Model
{
    using SkyMap.Net.Gui;
    using System;
    using System.ComponentModel;
    using System.Data.Common;

    public class ActdefMap
    {
        private int actdefID;
        private string actdefName;
        private string connectString;
        private DbProviderFactory dataFactory;
        private const string fzSql = "delete from out_map where actdef_id={0};\r\nINSERT INTO [OUT_MAP] ([ACTDEF_ID], [PROINST_SQL], [ACTINST_SQL], [Process], [DataSourceType], [InformationType], [Description])\r\nselect \r\n actdef_id, '', 'insert into out_actinst\r\n([PROINS_ID], [STAFF_ID], [START_DATE], [END_DATE], [ACTDEF_ID], [WILLFINISHDATE], [INSTANCE_TYPE], [OPERATE_TYPE], [ACTINS_STATUS], [CAN_TIME], [MONITOR_TIME], [ACTINS_TIME], [HANGUP_TIME], [ACTINS_MEM], [MONITOR_MEM], [ARRIVE_DATE], [Process], [Department], [Approver], [Detail], [ApproveDate], [IsAlreadyExchange], [DataSourceType], [CreateTimeStamp], [InformationType], [InformationTypeName])\r\nselect\r\na.[PROINS_ID],a.[STAFF_ID], [START_DATE],\r\n[END_DATE], a.[ACTDEF_ID], [WILLFINISHDATE],\r\n[INSTANCE_TYPE], [OPERATE_TYPE], [ACTINS_STATUS],\r\n[CAN_TIME], [MONITOR_TIME], [ACTINS_TIME],\r\n[HANGUP_TIME], [ACTINS_MEM], [MONITOR_MEM],\r\n[ARRIVE_DATE],\r\n@proc as Process,\r\n''国土局'' as Department,\r\nc.staff_name as Approver,\r\n''同意'' as Detail,\r\nisnull(a.end_date,getdate()) as ApproveDate,\r\n0 as IsAlreadyExchage,\r\n@dstype as DataSourceType, \r\ngetdate() as CreateTimeStamp,\r\n@infotype as InformationType,\r\n'''' as InformationTypeName\r\nfrom T_ACTINST a\r\ninner join T_ACTDEF b on a.ACTDEF_id=b.ACTDEF_ID\r\ninner join T_STAFF c on c.staff_id=a.staff_id\r\nwhere a.actins_id=@aid', actdef_name, 4, '03', '{1}'\r\nfrom t_actdef where actdef_id={0}";
        private bool insertOK;
        private string prodefName;
        private string proposorNameField;
        private const string sbSql = "delete from out_map where actdef_id ={0};\r\nINSERT INTO [OUT_MAP] ([ACTDEF_ID], [PROINST_SQL], [ACTINST_SQL], [Process], [DataSourceType], [InformationType], [Description])\r\nselect \r\n actdef_id, '', 'insert into out_actinst\r\n([PROINS_ID], [STAFF_ID], [START_DATE], [END_DATE], [ACTDEF_ID], [WILLFINISHDATE], [INSTANCE_TYPE], [OPERATE_TYPE], [ACTINS_STATUS], [CAN_TIME], [MONITOR_TIME], [ACTINS_TIME], [HANGUP_TIME], [ACTINS_MEM], [MONITOR_MEM], [ARRIVE_DATE], [Process], [Department], [Approver], [Detail], [ApproveDate], [IsAlreadyExchange], [DataSourceType], [CreateTimeStamp], [InformationType], [InformationTypeName])\r\nselect\r\na.[PROINS_ID],a.[STAFF_ID], [START_DATE],\r\n[END_DATE], a.[ACTDEF_ID], [WILLFINISHDATE],\r\n[INSTANCE_TYPE], [OPERATE_TYPE], [ACTINS_STATUS],\r\n[CAN_TIME], [MONITOR_TIME], [ACTINS_TIME],\r\n[HANGUP_TIME], [ACTINS_MEM], [MONITOR_MEM],\r\n[ARRIVE_DATE],\r\n@proc as Process,\r\n''国土局'' as Department,\r\nc.staff_name as Approver,\r\n''同意'' as Detail,\r\nisnull(a.end_date,getdate()) as ApproveDate,\r\n0 as IsAlreadyExchage,\r\n@dstype as DataSourceType, \r\ngetdate() as CreateTimeStamp,\r\n@infotype as InformationType,\r\n'''' as InformationTypeName\r\nfrom T_ACTINST a\r\ninner join T_ACTDEF b on a.ACTDEF_id=b.ACTDEF_ID\r\ninner join T_STAFF c on c.staff_id=a.staff_id\r\nwhere a.actins_id=@aid', actdef_name, 1, '02', prodef_name\r\nfrom t_actdef a\r\ninner join t_prodef b on a.prodef_id=b.prodef_id where actdef_id = {0}";
        private const string sjSql = "delete from out_map where actdef_id={0};\r\nINSERT INTO [OUT_MAP] ([ACTDEF_ID], [PROINST_SQL], [ACTINST_SQL], [Process], [DataSourceType], [InformationType], [Description])\r\nVALUES \r\n  ({0}, 'insert into out_proinst\r\nselect a.*,\r\n''{1}'' as ProjectName,\r\nb.{2} as ProposorName,\r\na.start_date as ApplyingDate,\r\n''国土局'' as AcceptDepartment,\r\n'''' as ProposorAddress,\r\n'''' as ProposorPostCode,\r\n'''' as ProposorMobileNo,\r\n'''' as ProposorPhone,\r\n'''' as ProposorEmail,\r\nb.online_id as ONLINE_ID\r\nfrom T_PROINST a\r\ninner join {3} b on a.project_id=b.ywid\r\nwhere a.proins_id=@pid', 'insert into out_actinst\r\n([PROINS_ID], [STAFF_ID], [START_DATE], [END_DATE], [ACTDEF_ID], [WILLFINISHDATE], [INSTANCE_TYPE], [OPERATE_TYPE], [ACTINS_STATUS], [CAN_TIME], [MONITOR_TIME], [ACTINS_TIME], [HANGUP_TIME], [ACTINS_MEM], [MONITOR_MEM], [ARRIVE_DATE], [Process], [Department], [Approver], [Detail], [ApproveDate], [IsAlreadyExchange], [DataSourceType], [CreateTimeStamp], [InformationType], [InformationTypeName])\r\nselect\r\na.[PROINS_ID],a.[STAFF_ID], [START_DATE],\r\n[END_DATE], a.[ACTDEF_ID], [WILLFINISHDATE],\r\n[INSTANCE_TYPE], [OPERATE_TYPE], [ACTINS_STATUS],\r\n[CAN_TIME], [MONITOR_TIME], [ACTINS_TIME],\r\n[HANGUP_TIME], [ACTINS_MEM], [MONITOR_MEM],\r\n[ARRIVE_DATE],\r\n@proc as Process,\r\n''国土局'' as Department,\r\nc.staff_name as Approver,\r\n''同意'' as Detail,\r\nisnull(a.end_date,getdate()) as ApproveDate,\r\n0 as IsAlreadyExchage,\r\n@dstype as DataSourceType, \r\ngetdate() as CreateTimeStamp,\r\n@infotype as InformationType,\r\n'''' as InformationTypeName\r\nfrom T_ACTINST a\r\ninner join T_ACTDEF b on a.ACTDEF_id=b.ACTDEF_ID\r\ninner join T_STAFF c on c.staff_id=a.staff_id\r\nwhere a.actins_id=@aid', '{4}', 2, '01', '{1}')";
        private ActdefType type = ActdefType.审办;
        private string yWTableName;

        public ActdefMap(DbProviderFactory dataFactory, string connectionString, string prodefName, string ywTableName)
        {
            this.dataFactory = dataFactory;
            this.connectString = connectionString;
            this.prodefName = prodefName;
            this.yWTableName = ywTableName;
        }

        [DisplayName("环节ID")]
        public int ActdefID
        {
            get
            {
                return this.actdefID;
            }
            set
            {
                this.actdefID = value;
                this.insertOK = false;
            }
        }

        [DisplayName("环节名称")]
        public string ActdefName
        {
            get
            {
                return this.actdefName;
            }
            set
            {
                this.actdefName = value;
                this.insertOK = false;
            }
        }

        [DisplayName("确认生成")]
        public bool InsertOK
        {
            get
            {
                return this.insertOK;
            }
            set
            {
                this.insertOK = value;
                if (this.insertOK && (this.dataFactory != null))
                {
                    using (DbConnection connection = this.dataFactory.CreateConnection())
                    {
                        connection.ConnectionString = this.connectString;
                        connection.Open();
                        DbCommand command = connection.CreateCommand();
                        switch (this.type)
                        {
                            case ActdefType.收件:
                                command.CommandText = string.Format("delete from out_map where actdef_id={0};\r\nINSERT INTO [OUT_MAP] ([ACTDEF_ID], [PROINST_SQL], [ACTINST_SQL], [Process], [DataSourceType], [InformationType], [Description])\r\nVALUES \r\n  ({0}, 'insert into out_proinst\r\nselect a.*,\r\n''{1}'' as ProjectName,\r\nb.{2} as ProposorName,\r\na.start_date as ApplyingDate,\r\n''国土局'' as AcceptDepartment,\r\n'''' as ProposorAddress,\r\n'''' as ProposorPostCode,\r\n'''' as ProposorMobileNo,\r\n'''' as ProposorPhone,\r\n'''' as ProposorEmail,\r\nb.online_id as ONLINE_ID\r\nfrom T_PROINST a\r\ninner join {3} b on a.project_id=b.ywid\r\nwhere a.proins_id=@pid', 'insert into out_actinst\r\n([PROINS_ID], [STAFF_ID], [START_DATE], [END_DATE], [ACTDEF_ID], [WILLFINISHDATE], [INSTANCE_TYPE], [OPERATE_TYPE], [ACTINS_STATUS], [CAN_TIME], [MONITOR_TIME], [ACTINS_TIME], [HANGUP_TIME], [ACTINS_MEM], [MONITOR_MEM], [ARRIVE_DATE], [Process], [Department], [Approver], [Detail], [ApproveDate], [IsAlreadyExchange], [DataSourceType], [CreateTimeStamp], [InformationType], [InformationTypeName])\r\nselect\r\na.[PROINS_ID],a.[STAFF_ID], [START_DATE],\r\n[END_DATE], a.[ACTDEF_ID], [WILLFINISHDATE],\r\n[INSTANCE_TYPE], [OPERATE_TYPE], [ACTINS_STATUS],\r\n[CAN_TIME], [MONITOR_TIME], [ACTINS_TIME],\r\n[HANGUP_TIME], [ACTINS_MEM], [MONITOR_MEM],\r\n[ARRIVE_DATE],\r\n@proc as Process,\r\n''国土局'' as Department,\r\nc.staff_name as Approver,\r\n''同意'' as Detail,\r\nisnull(a.end_date,getdate()) as ApproveDate,\r\n0 as IsAlreadyExchage,\r\n@dstype as DataSourceType, \r\ngetdate() as CreateTimeStamp,\r\n@infotype as InformationType,\r\n'''' as InformationTypeName\r\nfrom T_ACTINST a\r\ninner join T_ACTDEF b on a.ACTDEF_id=b.ACTDEF_ID\r\ninner join T_STAFF c on c.staff_id=a.staff_id\r\nwhere a.actins_id=@aid', '{4}', 2, '01', '{1}')", new object[] { this.actdefID, this.prodefName, this.proposorNameField, this.yWTableName, this.actdefName });
                                break;

                            case ActdefType.发件:
                                command.CommandText = string.Format("delete from out_map where actdef_id={0};\r\nINSERT INTO [OUT_MAP] ([ACTDEF_ID], [PROINST_SQL], [ACTINST_SQL], [Process], [DataSourceType], [InformationType], [Description])\r\nselect \r\n actdef_id, '', 'insert into out_actinst\r\n([PROINS_ID], [STAFF_ID], [START_DATE], [END_DATE], [ACTDEF_ID], [WILLFINISHDATE], [INSTANCE_TYPE], [OPERATE_TYPE], [ACTINS_STATUS], [CAN_TIME], [MONITOR_TIME], [ACTINS_TIME], [HANGUP_TIME], [ACTINS_MEM], [MONITOR_MEM], [ARRIVE_DATE], [Process], [Department], [Approver], [Detail], [ApproveDate], [IsAlreadyExchange], [DataSourceType], [CreateTimeStamp], [InformationType], [InformationTypeName])\r\nselect\r\na.[PROINS_ID],a.[STAFF_ID], [START_DATE],\r\n[END_DATE], a.[ACTDEF_ID], [WILLFINISHDATE],\r\n[INSTANCE_TYPE], [OPERATE_TYPE], [ACTINS_STATUS],\r\n[CAN_TIME], [MONITOR_TIME], [ACTINS_TIME],\r\n[HANGUP_TIME], [ACTINS_MEM], [MONITOR_MEM],\r\n[ARRIVE_DATE],\r\n@proc as Process,\r\n''国土局'' as Department,\r\nc.staff_name as Approver,\r\n''同意'' as Detail,\r\nisnull(a.end_date,getdate()) as ApproveDate,\r\n0 as IsAlreadyExchage,\r\n@dstype as DataSourceType, \r\ngetdate() as CreateTimeStamp,\r\n@infotype as InformationType,\r\n'''' as InformationTypeName\r\nfrom T_ACTINST a\r\ninner join T_ACTDEF b on a.ACTDEF_id=b.ACTDEF_ID\r\ninner join T_STAFF c on c.staff_id=a.staff_id\r\nwhere a.actins_id=@aid', actdef_name, 4, '03', '{1}'\r\nfrom t_actdef where actdef_id={0}", this.actdefID, this.prodefName);
                                break;

                            case ActdefType.审办:
                                command.CommandText = string.Format("delete from out_map where actdef_id ={0};\r\nINSERT INTO [OUT_MAP] ([ACTDEF_ID], [PROINST_SQL], [ACTINST_SQL], [Process], [DataSourceType], [InformationType], [Description])\r\nselect \r\n actdef_id, '', 'insert into out_actinst\r\n([PROINS_ID], [STAFF_ID], [START_DATE], [END_DATE], [ACTDEF_ID], [WILLFINISHDATE], [INSTANCE_TYPE], [OPERATE_TYPE], [ACTINS_STATUS], [CAN_TIME], [MONITOR_TIME], [ACTINS_TIME], [HANGUP_TIME], [ACTINS_MEM], [MONITOR_MEM], [ARRIVE_DATE], [Process], [Department], [Approver], [Detail], [ApproveDate], [IsAlreadyExchange], [DataSourceType], [CreateTimeStamp], [InformationType], [InformationTypeName])\r\nselect\r\na.[PROINS_ID],a.[STAFF_ID], [START_DATE],\r\n[END_DATE], a.[ACTDEF_ID], [WILLFINISHDATE],\r\n[INSTANCE_TYPE], [OPERATE_TYPE], [ACTINS_STATUS],\r\n[CAN_TIME], [MONITOR_TIME], [ACTINS_TIME],\r\n[HANGUP_TIME], [ACTINS_MEM], [MONITOR_MEM],\r\n[ARRIVE_DATE],\r\n@proc as Process,\r\n''国土局'' as Department,\r\nc.staff_name as Approver,\r\n''同意'' as Detail,\r\nisnull(a.end_date,getdate()) as ApproveDate,\r\n0 as IsAlreadyExchage,\r\n@dstype as DataSourceType, \r\ngetdate() as CreateTimeStamp,\r\n@infotype as InformationType,\r\n'''' as InformationTypeName\r\nfrom T_ACTINST a\r\ninner join T_ACTDEF b on a.ACTDEF_id=b.ACTDEF_ID\r\ninner join T_STAFF c on c.staff_id=a.staff_id\r\nwhere a.actins_id=@aid', actdef_name, 1, '02', prodef_name\r\nfrom t_actdef a\r\ninner join t_prodef b on a.prodef_id=b.prodef_id where actdef_id = {0}", this.actdefID);
                                break;
                        }
                        command.ExecuteNonQuery();
                        command.Dispose();
                        MessageHelper.ShowInfo("成功添加");
                    }
                }
            }
        }

        [DisplayName("业务名称")]
        public string ProdefName
        {
            get
            {
                return this.prodefName;
            }
            set
            {
                this.prodefName = value;
                this.insertOK = false;
            }
        }

        [DisplayName("申请人字段")]
        public string ProposorNameField
        {
            get
            {
                return this.proposorNameField;
            }
            set
            {
                this.proposorNameField = value;
                this.insertOK = false;
            }
        }

        [DisplayName("环节类型")]
        public ActdefType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
                this.insertOK = false;
            }
        }

        [DisplayName("数据表名称")]
        public string YWTableName
        {
            get
            {
                return this.yWTableName;
            }
            set
            {
                this.yWTableName = value;
                this.insertOK = false;
            }
        }
    }
}

