import Container from "./ui/Container";
import whyUsImage from "../assets/whayUseChart.avif";

export default function WhyUs() {
  const items = [
    {
      title: "استراتژی قبل از اجرا",
      description: "قبل از شروع هر پروژه، کسب‌وکار شما را تحلیل می‌کنیم.",
    },
    {
      title: "تمرکز روی نتیجه",
      description: "هدف ما فقط طراحی نیست؛ هدف رشد واقعی کسب‌وکار شماست.",
    },
    {
      title: "طراحی مدرن",
      description: "راهکارهای دیجیتال مطابق استانداردهای روز دنیا.",
    },
    {
      title: "پشتیبانی حرفه‌ای",
      description: "در تمام مسیر رشد همراه شما هستیم.",
    },
  ];

  return (
    <section
      className="
            py-1
            -mt-9
        "
    >
      {/* py-24 */}

      <Container>
        <div
          className="
            grid
            md:grid-cols-2
            gap-16
            items-center
            "
        >
          <div
            className="
                    w-64
                    h-64
                    sm:w-100
                    sm:h-100
                    lg:w-96
                    lg:h-96
                    rounded-full
                    overflow-hidden
                    mx-auto
                    "
          >
            <img
              src={whyUsImage}
              alt="WhyUs"
              className="
                        w-full
                        h-full
                        object-cover
                        "
            />
          </div>

          {/* Content */}

          <div>
            <h2
              className="
                    text-4xl
                    font-black
                    "
            >
              چرا رشدینو؟؟
            </h2>

            <p
              className="
                    mt-6
                    text-gray-600
                    leading-8
                    "
            >
              ما با ترکیب استراتژی، تکنولوژی و خلاقیت به کسب‌وکارها کمک می‌کنیم
              سریع‌تر رشد کنند.
            </p>

            <div
              className="
                    mt-10
                    space-y-6
                    "
            >
              {items.map((item) => (
                <div
                  key={item.title}
                  className="
                            flex
                            gap-4
                            "
                >
                  <div
                    className="
                                w-10
                                h-10
                                rounded-full
                                bg-purple-100
                                flex
                                items-center
                                justify-center
                                text-purple-600
                                "
                  >
                    ✓
                  </div>

                  <div>
                    <h3
                      className="
                                    font-bold
                                    text-lg
                                    "
                    >
                      {item.title}
                    </h3>

                    <p
                      className="
                                    text-gray-500
                                    mt-1
                                    "
                    >
                      {item.description}
                    </p>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </Container>
    </section>
  );
}
