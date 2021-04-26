using ModelFormBuilder;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceFormBuilder
{
    public interface IFormBuilderService
    {
		bool AddFormBuilder(FormBuilder request);

		bool UpdateFormBuilderbyId(FormBuilder request);
		IList<FormBuilder> UpdateFormBuilder(List<FormBuilder> tasks);

		IList<FormBuilder> DeleteFormBuilder(List<FormBuilder> tasks);

		List<FormBuilder> GetFormBuilder(int Id);
	}
}
