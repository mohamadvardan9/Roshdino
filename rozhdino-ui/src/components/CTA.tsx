{
  /*این بخش دعوت به اقدام است*/
  /*مثل :
    همین الان شروع کن
    ثبت نام کنید
    درخواست مشاوره
    شارژ کیف پول
    ساخت حساب کاربری 
    etc . . .
  */
}
{
  /*که بالای فوتر قرار دارد*/
}

import Container from "./ui/Container";

export default function CTA() {
  return (
    <section
      className="
        py-24
        "
    >
      <Container>
        <div
          className="
                rounded-3xl
                bg-gradient-to-br
                from-purple-600
                to-green-500
                p-12
                md:p-16
                text-center
                text-white
                "
        >
          <h2
            className="
                    text-4xl
                    md:text-5xl
                    font-black
                    "
          >
            آماده‌ای کسب‌وکارت را رشد بدهی؟
          </h2>

          <p
            className="
                    mt-6
                    text-lg
                    text-white/90
                    max-w-2xl
                    mx-auto
                    leading-8
                    "
          >
            با یک جلسه مشاوره رایگان، مسیر درست رشد دیجیتال کسب‌وکارت را پیدا کن
          </p>

          <button
            className="
                    mt-10
                    bg-white
                    text-purple-600
                    px-10
                    py-4
                    rounded-2xl
                    font-bold
                    hover:scale-105
                    transition
                    "
          >
            شروع همکاری
          </button>
        </div>
      </Container>
    </section>
  );
}
