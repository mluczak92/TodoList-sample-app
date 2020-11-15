using System;

namespace TodoList_sample_app.Models {
    interface IBoundriesSelector {
        public void Select(int year, int month, out DateTime min, out DateTime max);
    }
}
