namespace ShadowViewer.Interfaces
{
    public interface IDataBaseItem
    {
        // <summary>
        /// 添加到数据库,如果已存在,则更新数据
        /// </summary>
        public void Add();
        /// <summary>
        /// 在数据库中更新
        /// </summary>
        void Update();
        /// <summary>
        /// 从数据库中删除
        /// </summary>
         void Remove();
    }
}
