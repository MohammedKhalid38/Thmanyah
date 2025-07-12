export function LoadMultiLanguageFields() {
    if ($(document).find('multilingual').length > 0) {
        $(document).find('multilingual').each(function () {
            let element = $(this);
            $.post('/CustomFields/MultilingualField', {
                name: $(element).attr('name'),
                type: $(element).attr('type'),
                content: $(element).attr('data-content'),
                cssClass: $(element).attr('class'),
                cssStyle: $(element).attr('style'),
                isResizable: $(element).attr('resize')
            }, function (data) {
                $(element).html(data);
                RichTextEditorInitializer();
                MultilingualTagifyInitializer(element);
            });
        });
    }  
   
}